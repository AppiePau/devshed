namespace Devshed.Shared
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// Type to string converter helper class.
    /// </summary>
    public class TypeToStringConvertor
    {
        /// <summary>
        /// Converts the the variable to string.
        /// </summary>
        /// <param name="type"> Value type. </param>
        /// <param name="value"> The value to convert. </param>
        /// <param name="culture"> The culture to use. </param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual string AsString(Type type, object value, CultureInfo culture)
        {
            TypeConverter converter = TypeConvertorHelpers.GetConverterForType(type);

            return converter.ConvertToString(null, culture, value);
        }
    }
}
