namespace Devshed.Web.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class UrlBuilderTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            PageUrlBuilder.RegisterRootNamespace("Devshed.Web");
        }

        [TestMethod]
        public void Created_FromType_ReturnsPredictableUrl()
        {
            var builder = new UrlBuilder("http://www.example.com/");

            Assert.AreEqual("http://www.example.com/", builder.ToString());
        }

        [TestMethod]
        public void AddParameter_NewParameter_IsAddedToUrl()
        {
            var b = new UrlBuilder("http://www.example.com/");
            b.AddParameter("UserId", 1);

            Assert.AreEqual("http://www.example.com/?UserId=1", b.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddParameter_DuplicateParameterSameValue_ThrowsException()
        {
            var b = new UrlBuilder("http://www.example.com/");
            b.AddParameter("UserId", "1");
            b.AddParameter("UserId", "1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddParameter_DuplicateParameterDifferentValue_ThrowsException()
        {
            var b = new UrlBuilder("http://www.example.com/");
            b.AddParameter("UserId", "1");
            b.AddParameter("UserId", "2");
        }

        [TestMethod]
        public void MergeParameter_AddAndMergeDifferentValues_LastParameterOverwrites()
        {
            var b = new UrlBuilder("http://www.example.com/");
            b.AddParameter("UserId", "1");
            b.MergeParameter("UserId", "2");

            Assert.AreEqual("http://www.example.com/?UserId=2", b.ToString());
        }

        [TestMethod]
        public void MergeParameter_MergeWithoutAdding_LastParameterOverwrites()
        {
            var b = new UrlBuilder("http://www.example.com/");
            b.MergeParameter("UserId", "1");
            b.MergeParameter("UserId", "2");

            Assert.AreEqual("http://www.example.com/?UserId=2", b.ToString());
        }

        [TestMethod]
        public void Merge_TwoUrlBuilders_LastParameterOverwrites()
        {
            var merged = new UrlBuilder("http://www.merged.com/");
            merged.MergeParameter("UserId", "2");

            var target = new UrlBuilder("http://www.target.com/");
            target.MergeParameter("UserId", "1");

            target.Merge(merged);

            Assert.AreEqual("http://www.target.com/?UserId=2", target.ToString());
        }


        [TestMethod]
        public void AddParameter_NewAnonymousParameter_IsAddedToUrl()
        {
            var b = new UrlBuilder("http://www.example.com/", new { UserId = 1 });

            Assert.AreEqual("http://www.example.com/?UserId=1", b.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddParameter_DuplicateAnonymousParametersOfSameValue_ThrowsException()
        {
            var b = new UrlBuilder("http://www.example.com/");
            b.AddParameters(new { UserId = 1 });
            b.AddParameters(new { UserId = 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddParameter_DuplicateAnonymousParameterDifferentValue_ThrowsException()
        {
            var b = new UrlBuilder("http://www.example.com/");
            b.AddParameters(new { UserId = 1 });
            b.AddParameters(new { UserId = 2 });
        }

        [TestMethod]
        public void MergeParameter_AddAndMergeDifferentAnonymousParameters_LastParameterOverwrites()
        {
            var b = new UrlBuilder("http://www.example.com/");
            b.AddParameters(new { UserId = 1 });
            b.MergeParameters(new { UserId = 2 });

            Assert.AreEqual("http://www.example.com/?UserId=2", b.ToString());
        }

        [TestMethod]
        public void MergeParameter_MergeWithoutAddingAnonymousParameters_LastParameterOverwrites()
        {
            var b = new UrlBuilder("http://www.example.com/");
            b.MergeParameters(new { UserId = 1 });
            b.MergeParameters(new { UserId = 2 });

            Assert.AreEqual("http://www.example.com/?UserId=2", b.ToString());
        }

        [TestMethod]
        public void Merge_TwoUrlBuildersWithAnonymousParameters_LastParameterOverwrites()
        {
            var merged = new UrlBuilder("http://www.merged.com/");
            merged.MergeParameters(new { UserId = 2 });

            var target = new UrlBuilder("http://www.target.com/");
            target.MergeParameters(new { UserId = 1 });

            target.Merge(merged);

            Assert.AreEqual("http://www.target.com/?UserId=2", target.ToString());
        }


        [TestMethod]
        public void ParametersByConstructor_AnonymousParameters_AddedToAddress()
        {
            var b = new UrlBuilder("http://www.example.com/", new { UserId = 1 });

            Assert.AreEqual("http://www.example.com/?UserId=1", b.ToString());
        }

        [TestMethod]
        public void ParametersByConstructor_Array_IsConvertedToString()
        {
            var b = new UrlBuilder("http://www.example.com/", new { Users = new[] { 1, 2, 3 } });

            Assert.AreEqual("http://www.example.com/?Users=1%2c2%2c3", b.ToString());
        }
    }

    class Parameters
    {
        public string SearchText { get; set; }
    }

    [Serializable]
    class SerializableParameters : Parameters
    {
    }
}
