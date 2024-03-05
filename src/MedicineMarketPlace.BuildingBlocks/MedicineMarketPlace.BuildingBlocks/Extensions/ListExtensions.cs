namespace MedicineMarketPlace.BuildingBlocks.Extensions
{
    public static class ListExtensions
    {
        public static List<T> ToEmptyListIfNull<T>(this List<T> list)
        {
            return list ?? new List<T>();
        }
    }
}
