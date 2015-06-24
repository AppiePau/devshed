namespace Devshed.Web
{
    using System;
    using System.Globalization;

    internal static class CultureInvariantConverter
    {
        internal static string GetValue(object value)
        {
            if (value is IFormattable)
            {
                return ((IFormattable)value).ToString(null, CultureInfo.InvariantCulture);
            }

            return value.ToString();
        }
    }
}
