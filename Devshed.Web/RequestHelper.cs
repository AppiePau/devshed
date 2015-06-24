namespace Devshed.Web
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using Devshed.Shared;

    /// <summary>
    /// Web application helper methods.
    /// </summary>
    public static class RequestConverter
    {
        /// <summary>
        /// Defines a type safe method for retrieving a value from the query string. An exception is thrown
        /// when the value can not be converted. Use <see cref="Nullable{T}" /> for value types that can be
        /// empty.
        /// </summary>
        /// <typeparam name="T">The type to convert the string value from the querystring to.</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="System.FormatException">Cannot convert query parameter ' + parameterName + ' with value ' + value + '.</exception>
        /// <exception cref="FormatException">Thrown when the value can not be converted.</exception>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "Een ontwerp met 'object GetQueryStringValue(Type type, string name)' is " +
            "uiterst onhandig in deze situatie.")]
        [DebuggerStepThrough]
        public static T QueryString<T>(string parameterName, CultureInfo culture)
        {
            HttpRequest request = GetCurrentRequest();

            string value = request.QueryString[parameterName];
            try
            {
                return Conversion.AsValue<T>(value, culture);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Cannot convert query parameter '" + parameterName + "' with value '" + value + "'.", ex);
            }
        }

        public static T QueryString<T>(string parameterName)
        {
            return QueryString<T>(parameterName, Thread.CurrentThread.CurrentCulture);
        }

        public static string QueryString(string parameterName)
        {
            return QueryString<string>(parameterName);
        }

        /// <summary>
        /// Defines a type safe method for retrieving a value from the form. An exception is thrown
        /// when the value can not be converted. Use <see cref="Nullable{T}" /> for value types that can be
        /// empty.
        /// </summary>
        /// <typeparam name="T">The type to convert the string value from the form to.</typeparam>
        /// <param name="name">The name of the parameter in the connection string.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Could not retrieve form value with key ' + name + '. See inner exception for details.</exception>
        /// <exception cref="FormatException">Thrown when the value can not be converted.</exception>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "Een ontwerp met 'object GetFormValue(Type type, string name)' is " +
            "uiterst onhandig in deze situatie.")]
        [DebuggerStepThrough]
        public static T GetForm<T>(string name, CultureInfo culture)
        {
            try
            {
                HttpRequest request = GetCurrentRequest();

                string value = request.Form[name];

                return Conversion.AsValue<T>(value, culture);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not retrieve form value with key '" + name + "'. See inner exception for details.", ex);
            }
        }

        [DebuggerStepThrough]
        public static string Form(string name)
        {
            return GetForm<string>(name);
        }

        [DebuggerStepThrough]
        public static string GetForm(string name)
        {
            return GetForm<string>(name, Thread.CurrentThread.CurrentCulture);
        }

        [DebuggerStepThrough]
        public static T GetForm<T>(string name)
        {
            return GetForm<T>(name, Thread.CurrentThread.CurrentCulture);
        }

        private static HttpRequest GetCurrentRequest()
        {
            HttpContext context = HttpContext.Current;

            if (context == null)
            {
                throw new InvalidOperationException(
                    "There's no HttpContext.Current. Are you calling this in the context of a web " +
                    "application?");
            }

            HttpRequest request = context.Request;

            if (request == null)
            {
                throw new InvalidOperationException("The current context has no request.");
            }

            return request;
        }
    }
}
