namespace Devshed.Web
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary> Assists in building a path to a page with parameters. </summary>
    /// <example><![CDATA[
    /// Usage:
    ///        var builder = new UrlBuilder("Details.aspx");
    ///        builder.AddParameter("userid", 20);
    ///        builder.AddParameter("returnurl", "Users.aspx?selecteduserid=20");
    ///        EditUserHyperLink.NavigateUrl = builder.GetSafeUrl();
    /// Note:
    ///        The userid (20) is being hardtyped for this example.
    ///
    /// Result:
    ///        Details.aspx?userid=20&returnurl=Users.aspx%3fselecteduserid%3d20
    ///
    /// ]]></example>
    /// <remarks> Querystring values are encoded with the Anti XSS library from Microsoft. </remarks>
    public sealed class UrlBuilder
    {
        /// Copyright Paul Appeldoorn, Devshed.nl 2010. http://blog.devshed.nl ///

        private readonly string url;

        private readonly ParameterValueDictionary parameters;

        /// <summary> Initializes a new instance of the <see cref="UrlBuilder"/>. </summary>
        /// <param name="url">The name of the page, including extension. </param>
        public UrlBuilder(string url)
        {
            Requires.IsNotNullOrEmpty(url, "url");

            this.url = url;
            this.parameters = new ParameterValueDictionary();
        }

        /// <summary> Initializes a new instance of the <see cref="UrlBuilder"/>. </summary>
        /// <param name="url">The name of the page, including extension. </param>
        /// <param name="parameters"> The parameters. </param>
        public UrlBuilder(string url, params object[] parameters)
            : this(url)
        {
            Requires.IsNotNull(parameters, "parameters");

            foreach (var param in parameters)
            {
                this.parameters.Merge(new ParameterValueDictionary(param));
            }
        }

        /// <summary>
        /// Adds the source parameters to the builder.
        /// </summary>
        /// <param name="builder"> The builder to clone. </param>
        /// <remarks> Duplicate parameters are not allowed. </remarks>
        public UrlBuilder(UrlBuilder builder)
            : this(builder.url)
        {
            this.Merge(builder);
        }

        public UrlBuilder Merge(UrlBuilder source)
        {
            this.parameters.Merge(source.parameters);

            return this;
        }

        /// <summary>
        /// Adds the parameter and throws an exception when a key already exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.InvalidOperationException">The parameter key ' + key + ' already exists.</exception>
        public UrlBuilder AddParameter(string key, object value)
        {
            if (this.parameters.Keys.Contains(key))
            {
                throw new InvalidOperationException("The parameter key '" + key + "' already exists.");
            }

            this.parameters.Add(key, CultureInvariantConverter.GetValue(value));

            return this;
        }

        /// <summary>
        /// Adds the parameters and overwrites duplicates. The merged collection is leading.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.InvalidOperationException">The following keys already exist in the paramter collection:  +
        ///                 string.Join(\n - , duplicates.Select(d => d.Key).ToArray())</exception>
        public UrlBuilder AddParameters(object parameters)
        {
            this.AddParameters(new ParameterValueDictionary(parameters));

            return this;
        }

        /// <summary>
        /// Adds the parameters and overwrites duplicates. The merged collection is leading.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.InvalidOperationException">The following keys already exist in the paramter collection:  +
        ///                 string.Join(\n - , duplicates.Select(d => d.Key).ToArray())</exception>
        public UrlBuilder AddParameters(ParameterValueDictionary parameters)
        {
            var duplicates =
                from param in this.parameters
                where parameters.Keys.Contains(param.Key)
                select param;

            if (duplicates.Any())
            {
                throw new InvalidOperationException("The following keys already exist in the paramter collection: " +
                string.Join("\n - ", duplicates.Select(d => d.Key).ToArray()));
            }

            this.parameters.Merge(parameters);

            return this;
        }

        /// <summary> Adds a parameter to the page being called. </summary>
        /// <param name="name"> Name of the parameter. </param>
        /// <param name="value"> Value of the parameter. </param>
        public UrlBuilder MergeParameter(string name, object value)
        {
            Requires.IsNotNullOrEmpty(name, "name");
            Requires.IsNotNull(value, "value");

            this.parameters[name] = CultureInvariantConverter.GetValue(value);

            return this;
        }

        /// <summary> Adds a parameter to the page being called. </summary>
        /// <param name="parameters"> Parameters as anonymous object. </param>
        public UrlBuilder MergeParameters(object parameters)
        {
            Requires.IsNotNull(parameters, "parameters");

            this.MergeParameters(new ParameterValueDictionary(parameters));

            return this;
        }

        /// <summary> Adds a parameter to the page being called. </summary>
        /// <param name="parameters"> Parameters as anonymous object. </param>
        public UrlBuilder MergeParameters(ParameterValueDictionary parameters)
        {
            Requires.IsNotNull(parameters, "parameters");

            this.parameters.Merge(parameters);

            return this;
        }


        /// <summary> Builds and returns the actual url. </summary>
        /// <returns> The page name with encoded parameters. </returns>
        public string GetSafeUrl()
        {
            var finalUrl = new StringBuilder(this.url);

            if (this.parameters.Count > 0)
            {
                AppendParameters(finalUrl, this.parameters);
            }

            return finalUrl.ToString();
        }

        /// <summary> Returns a <see cref="System.String"/> that represents this instance. </summary>
        /// <returns> A <see cref="System.String"/> that represents this instance. </returns>
        public override string ToString()
        {
            return this.GetSafeUrl();
        }

        private void ValidateExistenceOfNewParameter(string name)
        {
            bool itemAlreadyExists =
                (from key in parameters.Keys
                 where key.ToUpper() == name.ToUpper()
                 select key).Any();

            if (itemAlreadyExists)
            {
                throw new InvalidOperationException(
                    "The parameter name already exists in the collection.");
            }
        }

        private static void AppendParameters(
            StringBuilder finalUrl,
            IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var iterator = parameters.GetEnumerator();
            iterator.MoveNext();
            finalUrl.Append(GrabAndCreateSafeParameter("?{0}={1}", iterator));

            while (iterator.MoveNext())
            {
                finalUrl.Append(GrabAndCreateSafeParameter("&{0}={1}", iterator));
            }
        }

        private static string GrabAndCreateSafeParameter(
            string formatString, IEnumerator<KeyValuePair<string, string>> parameterIterator)
        {
            var parameter = parameterIterator.Current;
            return string.Format(
                formatString,
                parameter.Key,
                Microsoft.Security.Application.Encoder.UrlEncode(parameter.Value));
        }
    }
}
