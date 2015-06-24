namespace Devshed.Web
{
    using System;

    internal static class Requires
    {
        internal static void IsNotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(name);
            }
        }

        internal static void IsNotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        internal static void StartsWith(string start, string value, string name)
        {
            if (!value.StartsWith(start))
            {
                throw new ArgumentException("Value '" + name + "' does not start with '" + start + "'.");
            }
        }
    }
}
