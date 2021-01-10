namespace Devshed.Web
{
    using System;
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// Template helper extensions.
    /// </summary>
    public static class TemplateControlExtension
    {
        /// <summary>
        /// Get the the full qualified adres from an URL.
        /// </summary>
        /// <param name="control"> The ASP.NET control the extend. </param>
        /// <param name="relativeOrAbsolute"> The URL, either relative or absolute. </param>
        /// <returns></returns>
        public static string FullQualifiedAddress(this TemplateControl control, string relativeOrAbsolute)
        {
            return RequestExtensions.FullQualifiedAddress(control.Page.Request, relativeOrAbsolute);
        }
    }

    public static class RequestExtensions
    {
        public static string FullQualifiedAddress(this HttpRequest request, string relativeOrAbsolute)
        {
            var url = request.Url;
            var serverAddress =
                url.Scheme + "://" +
                url.Host +
                (url.IsDefaultPort ? "" : ":" + url.Port);

            var uri = new Uri(relativeOrAbsolute, UriKind.RelativeOrAbsolute);

            if (uri.IsAbsoluteUri)
            {
                return serverAddress + relativeOrAbsolute;
            }

            // At this point, we know the url is relative.
            return serverAddress + VirtualPathUtility.ToAbsolute(relativeOrAbsolute);
        }
    }
}
