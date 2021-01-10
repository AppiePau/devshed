namespace Devshed.Shared
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;

    /// <summary>Helper methods for converting values.</summary>
    public static class Conversion
    {
        static Conversion()
        {
            ToConvertor = new StringToTypeConvertor();
            FromConvertor = new TypeToStringConvertor();
        }

        /// <summary>
        /// The convertor to prive String to Type conversions. StringToTypeConvertor by default.
        /// </summary>
        public static StringToTypeConvertor ToConvertor { get; set; }

        /// <summary>
        /// The convertor to prive String to Type conversions. TypeToStringConvertor by default.
        /// </summary>
        public static TypeToStringConvertor FromConvertor { get; set; }

        /// <summary>
        /// Enables conversion values for the given type from it's string representation.
        /// </summary>
        /// <typeparam name="T">The type to convert to. This can be a <see cref="Nullable"/> type.</typeparam>
        /// <param name="value">The string to convert.</param>
        /// <returns>An instance of the given <typeparamref name="T"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "A design with 'object ConvertFromString(Type type, string value)' is " +
            "very suitable in this API.")]
        //// [DebuggerStepThrough]
        public static T AsValue<T>(string value)
        {
            return AsValue<T>(value, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Enables conversion values for the given type from it's string representation.
        /// </summary>
        /// <typeparam name="T">The type to convert to. This can be a <see cref="Nullable" /> type.</typeparam>
        /// <param name="value">The string to convert.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>
        /// An instance of the given <typeparamref name="T" />.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "A design with 'object ConvertFromString(Type type, string value)' is " +
            "very suitable in this API.")]
        //// [DebuggerStepThrough]
        public static T AsValue<T>(string value, CultureInfo culture)
        {
            return (T)AsValue(typeof(T), value, culture);
        }

        /// <summary>
        /// Enables conversion values for the given type from it's string representation.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The string to convert.</param>
        /// <returns>
        /// An instance as an object.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "A design with 'object ConvertFromString(Type type, string value)' is " +
            "very suitable in this API.")]
        //// [DebuggerStepThrough]
        public static object AsValue(Type type, string value)
        {
            return AsValue(type, value, Thread.CurrentThread.CurrentCulture);
        }

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
        //// [DebuggerStepThrough]
        public static object AsValue(Type type, string value, CultureInfo culture)
        {
             return ToConvertor.AsValue(type, value, culture);
        }

        /// <summary>Converts an value to it's string representation.</summary>
        /// <remarks>Note that there is a corner case in which conversion is not correct in the case of using
        /// <see cref="Nullable{T}"/>. Converting an empty array and converting an array of one element with
        /// a value of null will convert to an empty string, while converting an empty string back will always
        /// result in an empty array.</remarks>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The string representation of the value.</returns>
        //// [DebuggerStepThrough]
        public static string AsString<T>(T value)
        {
            return AsString(value, Thread.CurrentThread.CurrentCulture);
        }

        //public static object AsValue(Type propertyType, string element, object culture)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Converts a strong type value to string using the specified culture.
        /// </summary>
        /// <typeparam name="T"> The type of value to convert. </typeparam>
        /// <param name="value"> The value itself. </param>
        /// <param name="culture"> The culture as conversion context. </param>
        /// <returns></returns>
        //// [DebuggerStepThrough]
        public static string AsString<T>(T value, CultureInfo culture)
        {
            return AsString(typeof(T), value, culture);
        }

        /// <summary>
        /// Converts value to string using the current thread culture.   
        /// </summary>
        /// <param name="type"> The type as Type to convert. </param>
        /// <param name="value"> The value itself. </param>
        /// <returns></returns>
        //// [DebuggerStepThrough]
        public static string AsString(Type type, object value)
        {
            return AsString(type, value, Thread.CurrentThread.CurrentCulture);
        }


        /// <summary>
        /// Converts value to string using the current thread culture. 
        /// </summary>
        /// <param name="type"> The type as Type to convert. </param>
        /// <param name="value"> The value itself. </param>
        /// <param name="culture"> The culture as conversion context. </param>
        /// <returns></returns>
        //// [DebuggerStepThrough]
        public static string AsString(Type type, object value, CultureInfo culture)
        {
            TypeConverter converter = TypeConvertorHelpers.GetConverterForType(type);

            return converter.ConvertToString(null, culture, value);
        }
    }
}
