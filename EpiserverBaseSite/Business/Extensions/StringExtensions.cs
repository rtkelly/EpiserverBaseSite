using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static int ToInt(this string value)
        {
            int.TryParse(value, out int result);
            return result;
        }

        public static long ToLong(this string value)
        {
            long.TryParse(value, out long result);
            return result;
        }

        public static decimal ToDecimal(this string value)
        {
            decimal.TryParse(value, out decimal result);
            return result;
        }

        public static DateTime ToDateTime(this string value)
        {
            DateTime.TryParse(value, out DateTime result);
            return result;
        }
    }
}