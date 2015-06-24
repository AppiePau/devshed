namespace Devshed.Web
{
    using System;
    using System.Web;
    using System.Web.UI;

    public sealed class PageUrlBuilder
    {
        public static PageUrlBuilder Builder { get; private set; }

        public static void RegisterRootNamespace(string root)
        {
            Builder = new PageUrlBuilder(root);
        }

        private readonly string root;

        public PageUrlBuilder(string root)
        {
            this.root = root.EndsWith(".") ? root : root + ".";
        }

        /// <summary> Creates the instance of the <see cref="UrlBuilder"/> bases on the namespace and
        /// class name. </summary>
        /// <remarks> Note that this methods depends on the matching of the namespace and class name to the
        /// real physical path. StyleCop will enforce namespaces to match the physical path. </remarks>
        /// <returns> An initialized instance of the <see cref="UrlBuilder"/> object ready to use. </returns>
        public UrlBuilder For<T>() where T : IHttpHandler
        {
            Type page = typeof(T);
            string name = page.FullName;

            if (name.StartsWith(this.root))
            {
                //// Cut off the prefix and replace dots by slashes:
                string fullPathToFile = name.Substring(this.root.Length).Replace(".", "/");
                string fullPagePath = string.Format("~/{0}.{1}", fullPathToFile, GetExtension(page));

                return new UrlBuilder(fullPagePath);
            }

            throw new InvalidOperationException("Wrong namespace for page '" + name + "', expected '" + this.root + "'.");
        }

        /// <summary>
        /// Creates an instance using a custom path, in case the namespace is different to the
        /// folder structure. Caution advised, consider changing the one of them so they match.
        /// </summary>
        /// <typeparam name="TPage">Type of page to be linked to.</typeparam>
        /// <param name="parameters">The parameters as anonymous type. Like: instance.For&lt;Details&gt;(new { UserId = 1 }); </param>
        /// <returns></returns>
        public UrlBuilder For<TPage>(params object[] parameters) where TPage : IHttpHandler
        {
            var builder = For<TPage>();

            foreach (var parameter in parameters)
            {
                RequestSerializer.AddParameters(builder, parameter);
            }

            return builder;
        }

        private static string GetExtension(Type T)
        {
            if (T.IsSubclassOf(typeof(Page)))
            {
                return "aspx";
            }

            return "ashx";
        }
    }
}
