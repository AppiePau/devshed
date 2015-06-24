using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Devshed.Shared.Tests
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void AsValue_MilitaryTime1800_ConvertsCorrectly()
        {
            var time = (TimeSpan)Conversion.AsValue(typeof(TimeSpan), "18:00");
            var expected = new TimeSpan(18, 00, 00);

            Assert.AreEqual(expected, time);
        }

        [TestMethod]
        public void AsValue_MilitaryTime900_ConvertsCorrectly()
        {
            var time = (TimeSpan)Conversion.AsValue(typeof(TimeSpan), "9:00");
            var expected = new TimeSpan(9, 00, 00);

            Assert.AreEqual(expected, time);
        }
    }
}
