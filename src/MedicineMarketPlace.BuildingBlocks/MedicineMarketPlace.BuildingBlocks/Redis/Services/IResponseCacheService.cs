namespace MedicineMarketPlace.BuildingBlocks.Redis.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);

        Task UpdateCacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);

        Task<string> GetCachedResponseAsync(string cacheKey);

        Task DeleteCacheResponseAsync(string cacheKey);
    }
}
