using MedicineMarketPlace.BuildingBlocks.Api.Models;
using MedicineMarketPlace.BuildingBlocks.Extensions;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using MedicineMarketPlace.BuildingBlocks.Redis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace MedicineMarketPlace.BuildingBlocks.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IResponseCacheService responseCacheService)
        {
            try
            {
                var userId = context.User.FindFirstValue(nameof(ApplicationUser.Id));
                if (userId.IsNotBlank())
                {
                    var cachedResponse = await responseCacheService.GetCachedResponseAsync(userId);
                    if (cachedResponse != null)
                    {
                        var deserialisedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(cachedResponse);
                        var accesstoken = await context.GetTokenAsync("access_token");
                        if (deserialisedResponse.ToString() != accesstoken.ToString())
                        {
                            context.Response.Clear();
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            await context.Response.WriteAsync("Unauthorized");
                            return;
                        }
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) // dev response
                    : new ApiException((int)HttpStatusCode.InternalServerError); // production response

                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, jsonOptions);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
