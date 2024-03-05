using MedicineMarketPlace.BuildingBlocks.Extensions;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using MedicineMarketPlace.BuildingBlocks.RateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Security.Claims;

namespace MedicineMarketPlace.BuildingBlocks.MiddleWare
{
    public class UserBaseRateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        public UserBaseRateLimitingMiddleware(RequestDelegate next, IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var rateLimitingDecorator = endpoint?.Metadata.GetMetadata<UserBaseLimitRequests>();

            if (rateLimitingDecorator is null)
            {
                await _next(context);
                return;
            }

            var key = GenerateClientKey(context);
            var clientStatistics = await GetClientStatisticsByKey(key);

            if (clientStatistics != null && DateTime.UtcNow < clientStatistics.LastSuccessfulResponseTime.AddMinutes(rateLimitingDecorator.Time) && clientStatistics.NumberOfRequestsCompletedSuccessfully == rateLimitingDecorator.MaxRequests)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                await context.Response.WriteAsync("TooManyRequests");
                return;
            }

            await UpdateClientStatisticsStorage(key, rateLimitingDecorator.MaxRequests);
            await _next(context);
        }

        private static string GenerateClientKey(HttpContext context) => $"{context.Request.Path}_{context.User.FindFirstValue(nameof(ApplicationUser.Id))}";

        private async Task<UserBaseClientStatistics> GetClientStatisticsByKey(string key) => await _cache.GetCacheValueAsync<UserBaseClientStatistics>(key);

        private async Task UpdateClientStatisticsStorage(string key, int maxRequests)
        {
            var clientStat = await _cache.GetCacheValueAsync<UserBaseClientStatistics>(key);

            if (clientStat != null)
            {
                clientStat.LastSuccessfulResponseTime = DateTime.UtcNow;

                if (clientStat.NumberOfRequestsCompletedSuccessfully == maxRequests)
                    clientStat.NumberOfRequestsCompletedSuccessfully = 1;

                else
                    clientStat.NumberOfRequestsCompletedSuccessfully++;

                await _cache.SetCahceValueAsync<UserBaseClientStatistics>(key, clientStat);
            }
            else
            {
                var clientStatistics = new UserBaseClientStatistics
                {
                    LastSuccessfulResponseTime = DateTime.UtcNow,
                    NumberOfRequestsCompletedSuccessfully = 1
                };

                await _cache.SetCahceValueAsync<UserBaseClientStatistics>(key, clientStatistics);
            }
        }
    }

    public class UserBaseClientStatistics
    {
        public DateTime LastSuccessfulResponseTime { get; set; }
        public int NumberOfRequestsCompletedSuccessfully { get; set; }
    }
}
