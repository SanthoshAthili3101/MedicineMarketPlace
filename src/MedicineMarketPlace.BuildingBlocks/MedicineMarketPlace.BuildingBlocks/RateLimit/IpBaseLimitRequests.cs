namespace MedicineMarketPlace.BuildingBlocks.RateLimit
{
    [AttributeUsage(AttributeTargets.Method)]
    public class IpBaseLimitRequests : Attribute
    {
        public int Time { get; set; }
        public int MaxRequests { get; set; }
    }
}
