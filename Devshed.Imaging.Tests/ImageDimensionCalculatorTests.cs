using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Devshed.Imaging.Tests
{
    [TestClass]
    public class ImageDimensionCalculatorTests
    {
        [TestMethod]
        public void GetDimensions_305x330To100x100_ReturnsExpectedSizes()
        {
            var calc = new ImageDimensionCalculator(new Size(305, 330));

            var fitratio = calc.GetDimensions(100, 100, SizeMode.FitRatio);
            var fitratiofilled = calc.GetDimensions(100, 100, SizeMode.FitRatioFilled);
            var stretch = calc.GetDimensions(100, 100, SizeMode.Stretch);

            Assert.AreEqual(92, fitratio.Render.Width);
            Assert.AreEqual(100, fitratio.Render.Height);
            Assert.AreEqual(92, fitratio.Canvas.Width);
            Assert.AreEqual(100, fitratio.Canvas.Height);


            Assert.AreEqual(92, fitratiofilled.Render.Width);
            Assert.AreEqual(100, fitratiofilled.Render.Height);
            Assert.AreEqual(100, fitratiofilled.Canvas.Width);
            Assert.AreEqual(100, fitratiofilled.Canvas.Height);


            Assert.AreEqual(100, stretch.Render.Width);
            Assert.AreEqual(100, stretch.Render.Height);
            Assert.AreEqual(100, stretch.Canvas.Width);
            Assert.AreEqual(100, stretch.Canvas.Height);
        }

        [TestMethod]
        public void TestMethod2GetDimensions_1920x1080To100x100_ReturnsExpectedSizes()
        {
            var calc = new ImageDimensionCalculator(new Size(1920, 1080));

            var fitratio = calc.GetDimensions(100, 100, SizeMode.FitRatio);
            var fitratiofilled = calc.GetDimensions(100, 100, SizeMode.FitRatioFilled);
            var stretch = calc.GetDimensions(100, 100, SizeMode.Stretch);
            var cropped = calc.GetDimensions(100, 100, SizeMode.Crop);

            Assert.AreEqual(100, fitratio.Canvas.Width);
            Assert.AreEqual(56, fitratio.Canvas.Height);
            Assert.AreEqual(100, fitratio.Render.Width);
            Assert.AreEqual(56, fitratio.Render.Height);
     
            Assert.AreEqual(100, fitratiofilled.Canvas.Width);
            Assert.AreEqual(100, fitratiofilled.Canvas.Height);
            Assert.AreEqual(100, fitratiofilled.Render.Width);
            Assert.AreEqual(56, fitratiofilled.Render.Height);
            
            Assert.AreEqual(100, stretch.Canvas.Width);
            Assert.AreEqual(100, stretch.Canvas.Height);
            Assert.AreEqual(100, stretch.Render.Width);
            Assert.AreEqual(100, stretch.Render.Height);
            
            Assert.AreEqual(100, cropped.Canvas.Width);
            Assert.AreEqual(100, cropped.Canvas.Height);
            Assert.AreEqual(100, cropped.Render.Width);
            Assert.AreEqual(100, cropped.Render.Height);
        }
    }
}
