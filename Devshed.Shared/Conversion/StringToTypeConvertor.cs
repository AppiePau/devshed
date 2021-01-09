namespace Devshed.Shared
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    /// <summary>
    /// String to type converter helper class.
    /// </summary>
    public class StringToTypeConvertor
    {
        /// <summary>
        /// Enables conversion values for the given type from it's string representation.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The string to convert.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>
        /// An instance as an object.
        /// </returns>
        /// <exception cref="System.FormatException">The supplied value ' + value + ' could not be converted.  + ex.Message</exception>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "A design with 'object ConvertFromString(Type type, string value)' is " +
            "very suitable in this API.")]
        [DebuggerStepThrough]
        public virtual object AsValue(Type type, string value, CultureInfo culture)
        {
            return ConvertFromString(type, value, culture);
        }

        private object ConvertFromString(Type type, string value, CultureInfo culture)
        {
            TypeConverter converter = TypeConvertorHelpers.GetConverterForType(type);

            try
            {
                return converter.ConvertFromString(null, culture, value);
            }
            catch (Exception ex)
            {
                // HACK: There is a bug in the .NET framework (3.5sp1) BaseNumberConverter class. The
                // class throws an Exception base class, and therefore we must catch the 'Exception' base class.
                throw new FormatException(
                    "The supplied value '" + value + "' could not be converted. " + ex.Message, ex);
            }
        }
    }
}