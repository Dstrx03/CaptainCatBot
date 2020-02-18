using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cat.Common.Converters
{
    public class CatTimeSpanConverter
    {
        public static string Convert(TimeSpan timeSpan)
        {
            var weeks = timeSpan.TotalDays / 7;
            var days = timeSpan.TotalDays;
            var hours = timeSpan.TotalHours;
            var minutes = timeSpan.TotalMinutes;

            if (Math.Abs(weeks % 1) <= Double.Epsilon * 100 && days >= 7) return weeks + "w";
            if (Math.Abs(days % 1) <= Double.Epsilon * 100 && hours >= 24) return days + "d";
            if (Math.Abs(hours % 1) <= Double.Epsilon * 100 && minutes >= 60) return hours + "h";
            return minutes + "m";
        }

        public static TimeSpan Convert(string timeSpanString)
        {
            var regex = new Regex("^\\d+[mhdw]$");
            if (!regex.IsMatch(timeSpanString)) throw new ArgumentException(string.Format("Invalid timeSpanString format: {0}", timeSpanString), timeSpanString);

            var value = Int32.Parse(timeSpanString.Substring(0, timeSpanString.Length - 1));
            var kind = timeSpanString.Substring(timeSpanString.Length - 1);

            switch (kind)
            {
                case "w":
                    return TimeSpan.FromDays(value * 7);
                case "d":
                    return TimeSpan.FromDays(value);
                case "h":
                    return TimeSpan.FromHours(value);
                case "m":
                    return TimeSpan.FromMinutes(value);
                default:
                    throw new ArgumentException(string.Format("Invalid kind parsed: {0}", kind), kind);
            }
        }
    }
}
