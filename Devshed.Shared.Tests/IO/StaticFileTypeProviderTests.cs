namespace Devshed.Shared.Tests
{
    using System;
    using Devshed.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StaticFileTypeProviderTests
    {
        [TestMethod]
        public void RequestContentType_JPG_ReturnsImageJpg()
        {
            var provider = new StaticFileTypeProvider();

            var mime = provider.GetMimeType(".jpg");
            
            Assert.AreEqual("image/jpeg", mime.ContentType);
        }

        [TestMethod]
        public void RequestContentType_PNG_ReturnsApplicationOctetStream()
        {
            var provider = new StaticFileTypeProvider();

            var mime = provider.GetMimeType(".png");
            
            Assert.AreEqual("image/png", mime.ContentType);
        }

        [TestMethod]
        public void RequestContentType_EXE_ReturnsApplicationOctetStream()
        {
            var provider = new StaticFileTypeProvider();

            var mime = provider.GetMimeType(".exe");
            
            Assert.AreEqual("application/octet-stream", mime.ContentType);
        }

        [TestMethod]
        public void RequestContentType_UnknownType_ReturnsApplicationOctetStream()
        {
            var provider = new StaticFileTypeProvider();

            var mime = provider.GetMimeType(".unknown");
            
            Assert.AreEqual("application/octet-stream", mime.ContentType);
        }
    }
}
