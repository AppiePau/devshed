namespace Devshed.Web.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class PageUrlBuilderTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            PageUrlBuilder.RegisterRootNamespace("Devshed.Web");
        }

        [TestMethod]
        public void Created_FromType_ReturnsPredictableUrl()
        {
            var b = PageUrlBuilder.Builder.For<TargetTestPage>();

            Assert.AreEqual(b.ToString(), "~/Tests/TargetTestPage.ashx");
        }

        [TestMethod]
        public void AddParameter_NewParameter_IsAddedToUrl()
        {
            var b = PageUrlBuilder.Builder.For<TargetTestPage>();
            b.AddParameter("UserId", "1");

            Assert.AreEqual(b.ToString(), "~/Tests/TargetTestPage.ashx?UserId=1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddParameter_DuplicateParameterSameValue_ThrowsException()
        {
            var b = PageUrlBuilder.Builder.For<TargetTestPage>();
            b.AddParameter("UserId", "1");
            b.AddParameter("UserId", "1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddParameter_DuplicateParameterDifferentValue_ThrowsException()
        {
            var b = PageUrlBuilder.Builder.For<TargetTestPage>();
            b.AddParameter("UserId", "1");
            b.AddParameter("UserId", "2");
        }

        [TestMethod]
        public void MergeParameter_AddAndMergeDifferentValues_LastParameterOverwrites()
        {
            var b = PageUrlBuilder.Builder.For<TargetTestPage>();
            b.AddParameter("UserId", "1");
            b.MergeParameter("UserId", "2");
        
            Assert.AreEqual(b.ToString(), "~/Tests/TargetTestPage.ashx?UserId=2");
        }

        [TestMethod]
        public void MergeParameter_MergeWithoutAdding_LastParameterOverwrites()
        {
            var b = PageUrlBuilder.Builder.For<TargetTestPage>();
            b.MergeParameter("UserId", "1");
            b.MergeParameter("UserId", "2");

            Assert.AreEqual(b.ToString(), "~/Tests/TargetTestPage.ashx?UserId=2");
        }

        [TestMethod]
        public void Merge_TwoUrlBuilders_LastParameterOverwrites()
        {
            var merged = PageUrlBuilder.Builder.For<TargetTestPage>();
            merged.MergeParameter("UserId", "2");
            
            var target = PageUrlBuilder.Builder.For<TargetTestPage>();
            target.MergeParameter("UserId", "1");

            target.Merge(merged);

            Assert.AreEqual(target.ToString(), "~/Tests/TargetTestPage.ashx?UserId=2");
        }

        
        [TestMethod]
        public void AddParameter_NewAnonymousParameter_IsAddedToUrl()
        {
            var b = PageUrlBuilder.Builder.For<TargetTestPage>(new { UserId = 1 });

            Assert.AreEqual(b.ToString(), "~/Tests/TargetTestPage.ashx?UserId=1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddParameter_DuplicateAnonymousParametersOfSameValue_ThrowsException()
        {
            var b = PageUrlBuilder.Builder.For<TargetTestPage>();
            b.AddParameters(new { UserId = 1 });
            b.AddParameters(new { UserId = 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddParameter_DuplicateAnonymousParameterDifferentValue_ThrowsException()
        {
            var b = PageUrlBuilder.Builder.For<TargetTestPage>();
            b.AddParameters(new { UserId = 1 });
            b.AddParameters(new { UserId = 2 });
        }

        [TestMethod]
        public void MergeParameter_AddAndMergeDifferentAnonymousParameters_LastParameterOverwrites()
        {
            var b = PageUrlBuilder.Builder.For<TargetTestPage>();
            b.AddParameters(new { UserId = 1 });
            b.MergeParameters(new { UserId = 2 });
        
            Assert.AreEqual(b.ToString(), "~/Tests/TargetTestPage.ashx?UserId=2");
        }

        [TestMethod]
        public void MergeParameter_MergeWithoutAddingAnonymousParameters_LastParameterOverwrites()
        {
            var b = PageUrlBuilder.Builder.For<TargetTestPage>();
            b.MergeParameters(new { UserId = 1 });
            b.MergeParameters(new { UserId = 2 });

            Assert.AreEqual(b.ToString(), "~/Tests/TargetTestPage.ashx?UserId=2");
        }

        [TestMethod]
        public void Merge_TwoUrlBuildersWithAnonymousParameters_LastParameterOverwrites()
        {
            var merged = PageUrlBuilder.Builder.For<TargetTestPage>();
            merged.MergeParameters(new { UserId = 2 });

            var target = PageUrlBuilder.Builder.For<TargetTestPage>();
            target.MergeParameters(new { UserId = 1 });

            target.Merge(merged);

            Assert.AreEqual(target.ToString(), "~/Tests/TargetTestPage.ashx?UserId=2");
        }
    }
}
