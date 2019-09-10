using System;
using System.Globalization;
using Microsoft.Ajax.Utilities;

namespace WebApplication1
{
    public static class Utils
    {
        public static bool IsNotEmpty(this string str)
        {
            return !(string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str));
        }

        public static string ToSQL(this DateTime date)
        {
            return date.ToString("MM.dd.yyyy", CultureInfo.InvariantCulture);
        }
    }
}
