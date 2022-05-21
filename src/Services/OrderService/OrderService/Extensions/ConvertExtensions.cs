using System;

namespace OrderService.Extensions
{
    public static class ConvertExtensions
    {
        public static int ToInt(this string val, int defaultValue = default)
        {
            return Int32.TryParse(val, out var res) ? res : defaultValue;
        }
    }
}