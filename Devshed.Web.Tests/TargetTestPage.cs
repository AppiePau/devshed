namespace Devshed.Web.Tests
{
    using System;
    using System.Web;

    public class TargetTestPage : IHttpHandler
    {
        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
