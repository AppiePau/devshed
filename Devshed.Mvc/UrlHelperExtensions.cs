namespace Devshed.Mvc
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// URL helper extension to create server sided URLs.
    /// </summary>
    public static class UrlHelperExtension
    {
        /// <summary>
        /// Creates a full qulified address, including port, from a releative address.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="relativeOrAbsolute">The relative or absolute path.</param>
        /// <returns></returns>
        public static string FullQualifiedAddress(this UrlHelper urlHelper, string relativeOrAbsolute)
        {
            return RequestExtensions.FullQualifiedAddress(urlHelper.RequestContext.HttpContext.Request, relativeOrAbsolute);
        }
    }

    /// <summary>
    /// Request helper extensions. 
    /// </summary>
    public static class RequestExtensions
    {
        /// <summary>
        /// Creates a full qulified address, including port, from a releative address.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="relativeOrAbsolute">The relative or absolute path.</param>
        /// <returns></returns>
        public static string FullQualifiedAddress(this HttpRequestBase request, string relativeOrAbsolute)
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
