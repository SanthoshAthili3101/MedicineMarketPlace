namespace MedicineMarketPlace.BuildingBlocks.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateRange GetDateRange(this DateFilter filter)
        {
            var response = new DateRange();
            DateTime today = DateTime.Today;
            switch (filter)
            {
                case DateFilter.ThisWeek:
                    response.FromDate = today.AddDays(-((int)today.DayOfWeek));
                    response.ToDate = response.FromDate.AddDays(7).AddSeconds(-1);
                    return response;

                case DateFilter.ThisMonth:
                    response.FromDate = new DateTime(today.Year, today.Month, 1);
                    response.ToDate = response.FromDate.AddMonths(1).AddSeconds(-1);
                    return response;

                case DateFilter.ThisYear:
                    response.FromDate = new DateTime(today.Year, 1, 1);
                    response.ToDate = response.FromDate.AddYears(1).AddSeconds(-1);
                    return response;

                default:
                    return null;
            }
        }

        public static DateTime AddTimeInFromDate(this DateTime dateTime)
        {
            return dateTime.AddHours(0).AddMinutes(0).AddSeconds(0);
        }

        public static DateTime AddTimeInToDate(this DateTime dateTime)
        {
            return dateTime.AddHours(23).AddMinutes(59).AddSeconds(59);
        }
    }

    public class DateRange
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public enum DateFilter
    {
        ThisWeek,
        ThisMonth,
        ThisYear
    }
}
