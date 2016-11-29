namespace Devshed.Web.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Text.RegularExpressions;

    [TestClass]
    public class BasicPasswordExpressionTests
    {
        private Regex validator;

        [TestInitialize]
        public void TestInitialize()
        {
            this.validator = new Regex(ControlValidationExtensions.BasicPasswordExpression(6));
        }

        [TestMethod]
        public void BasicPassword_MinimumLengthSpecialChars_IsValid()
        {
            Assert.IsTrue(validator.IsMatch("Abc12!"));
            Assert.IsTrue(validator.IsMatch("Abc12@"));
            Assert.IsTrue(validator.IsMatch("Abc12#"));
            Assert.IsTrue(validator.IsMatch("Abc12$"));
            Assert.IsTrue(validator.IsMatch("Abc12%"));
            Assert.IsTrue(validator.IsMatch("Abc12^"));
            Assert.IsTrue(validator.IsMatch("Abc12&"));
            Assert.IsTrue(validator.IsMatch("Abc12*"));
            Assert.IsTrue(validator.IsMatch("Abc12("));
            Assert.IsTrue(validator.IsMatch("Abc12)"));
            Assert.IsTrue(validator.IsMatch("Abc12-"));
            Assert.IsTrue(validator.IsMatch("Abc12_"));
            Assert.IsTrue(validator.IsMatch("Abc12="));
            Assert.IsTrue(validator.IsMatch("Abc12+"));
            Assert.IsTrue(validator.IsMatch("Abc12;"));
            Assert.IsTrue(validator.IsMatch("Abc12:"));
            Assert.IsTrue(validator.IsMatch("Abc12'"));
            Assert.IsTrue(validator.IsMatch("Abc12\""));
            Assert.IsTrue(validator.IsMatch("Abc12|"));
            Assert.IsTrue(validator.IsMatch("Abc12\\"));
            Assert.IsTrue(validator.IsMatch("Abc12?"));
            Assert.IsTrue(validator.IsMatch("Abc12/"));
            Assert.IsTrue(validator.IsMatch("Abc12<"));
            Assert.IsTrue(validator.IsMatch("Abc12>"));
            Assert.IsTrue(validator.IsMatch("Abc12,"));
            Assert.IsTrue(validator.IsMatch("Abc12~"));
        }

        
        [TestMethod]
        public void BasicPassword_ValidToShort_IsNotValid()
        {
            Assert.IsFalse(validator.IsMatch("Abc1!"));
            Assert.IsFalse(validator.IsMatch("Abc1@"));
            Assert.IsFalse(validator.IsMatch("Abc1#"));
            Assert.IsFalse(validator.IsMatch("Abc1$"));
            Assert.IsFalse(validator.IsMatch("Abc1%"));
            Assert.IsFalse(validator.IsMatch("Abc1^"));
            Assert.IsFalse(validator.IsMatch("Abc1&"));
            Assert.IsFalse(validator.IsMatch("Abc1*"));
            Assert.IsFalse(validator.IsMatch("Abc1("));
            Assert.IsFalse(validator.IsMatch("Abc1)"));
            Assert.IsFalse(validator.IsMatch("Abc1-"));
            Assert.IsFalse(validator.IsMatch("Abc1_"));
            Assert.IsFalse(validator.IsMatch("Abc1="));
            Assert.IsFalse(validator.IsMatch("Abc1+"));
            Assert.IsFalse(validator.IsMatch("Abc1;"));
            Assert.IsFalse(validator.IsMatch("Abc1:"));
            Assert.IsFalse(validator.IsMatch("Abc1'"));
            Assert.IsFalse(validator.IsMatch("Abc1\""));
            Assert.IsFalse(validator.IsMatch("Abc1|"));
            Assert.IsFalse(validator.IsMatch("Abc1\\"));
            Assert.IsFalse(validator.IsMatch("Abc1?"));
            Assert.IsFalse(validator.IsMatch("Abc1/"));
            Assert.IsFalse(validator.IsMatch("Abc1<"));
            Assert.IsFalse(validator.IsMatch("Abc1>"));
            Assert.IsFalse(validator.IsMatch("Abc1,"));
            Assert.IsFalse(validator.IsMatch("Abc1~"));
        }

        [TestMethod]
        public void BasicPassword_ValidNoSpecialChar_IsNotValid()
        {
            Assert.IsFalse(validator.IsMatch("Abc123"));
        }
    }
}
