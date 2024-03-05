namespace MedicineMarketPlace.BuildingBlocks.RateLimit
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UserBaseLimitRequests : Attribute
    {
        public int Time { get; set; }
        public int MaxRequests { get; set; }
    }
}
