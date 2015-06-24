namespace Devshed.Web
{
    using System;
    using System.Web;
    using System.Web.UI;

    public static class TemplateControlExtension
    {
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
