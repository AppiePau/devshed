namespace Devshed.Web
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using Devshed.Shared;

    /// <summary> Serializes objects to urls (using the UrlBuilder) or deserializes from querystring or post variables. </summary>
    public static partial class RequestSerializer
    {
        private static PageUrlBuilder Factory { get { return PageUrlBuilder.Builder; } }

        /// <summary>
        /// Serializes the object(s) to an URL using the <see cref="UrlBuilder"/>.
        /// </summary>
        /// <typeparam name="TPage">The type of page.</typeparam>
        /// <param name="parameters">The parameters to add to the page URL.</param>
        /// <returns></returns>
        public static UrlBuilder ToUrlBuilder<TPage>(params object[] parameters) where TPage : IHttpHandler
        {
            var builder = Factory.For<TPage>();

            AddParameters(builder, parameters);

            return builder;
        }

        internal static void AddParameters(UrlBuilder builder, params object[] parameters)
        {
            foreach (var param in parameters)
            {
                var dictionary = new ParameterValueDictionary(param);
                builder.MergeParameters(dictionary);
            }
        }
    }

}