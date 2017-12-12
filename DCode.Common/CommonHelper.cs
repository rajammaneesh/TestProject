using System;

namespace DCode.Common
{
    public static class CommonHelper
    {
        public static string ConvertToDateUI(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                var strDate = String.Format("{0:dddd, MMMM d, yyyy}", dateTime);
                return strDate.Substring(strDate.IndexOf(',')+ 1); 
            }
            return string.Empty;
        }

        public static DateTime ConvertToDateDatabase(string dateTime)
        {
            if(!String.IsNullOrEmpty(dateTime))
            {
                return Convert.ToDateTime(dateTime);
            }
            return DateTime.MinValue;
        }

        public static string CalculateDuration(DateTime? createdDatetime)
        {
            if (createdDatetime != null)
            {
                var dateDiff = DateTime.Now - Convert.ToDateTime(createdDatetime);
                if (dateDiff.Days > 0 && dateDiff.Days > 10)
                {
                    return ConvertToDateUI(createdDatetime);
                }
                else if (dateDiff.Days > 0 && dateDiff.Days < 10)
                {
                    return dateDiff.Days + (dateDiff.Days == 1 ? Constants.DayAgo : Constants.DaysAgo);
                }
                else if (dateDiff.Days == 0 && dateDiff.Hours > 0)
                {
                    return dateDiff.Hours + (dateDiff.Hours == 1 ? Constants.HourAgo : Constants.HoursAgo);
                }
                else if (dateDiff.Hours == 0 && dateDiff.Minutes > 0)
                {
                    return dateDiff.Minutes + (dateDiff.Minutes == 1 ? Constants.MinuteAgo : Constants.MinutesAgo);
                }
                return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
