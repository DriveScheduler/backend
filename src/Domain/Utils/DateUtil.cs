namespace Domain.ValueObjects
{
    public static class DateUtil
    {
        
        public static DateTime GetFirstDayOfWeek(DateTime date)
        {            
            return date.AddDays(-1 * (date.DayOfWeek - DayOfWeek.Monday)).Date;
        }

        public static DateTime GetLastDayOfWeek(DateTime date)
        {
            return date.AddDays((DayOfWeek.Friday - date.DayOfWeek)).Date;
        }       
        
        public static DateTime GetLastDayOfMonth(DateTime date)
        {
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return new DateTime(date.Year, date.Month, daysInMonth);
        }

    }
}
