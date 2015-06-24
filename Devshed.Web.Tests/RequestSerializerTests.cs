namespace Devshed.Web.Tests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Web;
    using System.Globalization;

    [TestClass]
    public class RequestSerializerTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            PageUrlBuilder.RegisterRootNamespace("Devshed.Web");
        }

        [TestMethod]
        public void ToUrlBuilder_TwoParameterObjects_AddedInUrl()
        {
            var target = RequestSerializer.ToUrlBuilder<TargetTestPage>(new { UserId = 1 }, new { SearchText = "OK_TEXT" });
            string targetUrl = target.ToString();

            Assert.AreEqual("~/Tests/TargetTestPage.ashx?UserId=1&SearchText=OK_TEXT", targetUrl);
        }

        [TestMethod]
        public void InternalDeserialize_TwoParameters_ReturnObjectInstantiatedWithParameters()
        {
            var request = new HttpRequest(
                "TargetTestPage.ashx",
                "http://local/Tests/TargetTestPage.ashx",
                "UserId=1&SearchText=OK_TEXT");

            var source = RequestDeserializer.InternalDeserialize<TestParameters>(request.QueryString, CultureInfo.InvariantCulture);

            Assert.AreEqual(1, source.UserId);
            Assert.AreEqual("OK_TEXT", source.SearchText);
        }


        [TestMethod]
        public void InternalDeserialize_Array_ReturnArray()
        {
            var request = new HttpRequest(
                "TargetTestPage.ashx",
                "http://local/Tests/TargetTestPage.ashx",
                "Users=1,2,3&SearchText=OK_TEXT");

            var source = RequestDeserializer.InternalDeserialize<TestArrayParameters>(request.QueryString, CultureInfo.InvariantCulture);

            Assert.IsTrue((new[] { 1, 2, 3 }).SequenceEqual(source.Users));
            Assert.AreEqual("OK_TEXT", source.SearchText);
        }
    }
}