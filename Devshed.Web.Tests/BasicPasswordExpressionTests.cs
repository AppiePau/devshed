namespace Devshed.Web.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Text.RegularExpressions;
    using System.Web.UI.WebControls;
    using System.Web;
    using System.Text;
    using System.Web.UI;
    using System.IO;
    using System.Web.UI.HtmlControls;

    public class TestablePage : System.Web.UI.Page
    {

        private HttpContextBase _Context;

        public new System.Web.HttpContextBase Context
        {

            get
            {

                if (_Context == null) _Context = new System.Web.HttpContextWrapper(System.Web.HttpContext.Current);
                return _Context;

            }
            set { _Context = value; }

        }

    }
    [TestClass]
    public class PasswordAdvisorTests
    {
        [TestMethod]
        public void CheckStrength_Blank_ReturnsVeryBlank()
        {
            var result = PasswordAdvisor.CheckStrength("abcd");
            Assert.AreEqual(PasswordScore.Blank, result.Score);
        }

        [TestMethod]
        public void CheckStrength_VeryWeak_ReturnsVeryWeak1()
        {
            var result = PasswordAdvisor.CheckStrength("abcd1");
            Assert.AreEqual(PasswordScore.VeryWeak, result.Score);
        }

        [TestMethod]
        public void CheckStrength_Weak_ReturnsWeak()
        {
            var result = PasswordAdvisor.CheckStrength("Abcd1");
            Assert.AreEqual(PasswordScore.Weak, result.Score);
        }

        [TestMethod]
        public void CheckStrength_Medium_ReturnsWeak()
        {
            var result = PasswordAdvisor.CheckStrength("Abcd1!");
            Assert.AreEqual(PasswordScore.Medium, result.Score);
        }
    }

    [TestClass]
    public class BasicPasswordExpressionTests
    {
   
        [TestInitialize]
        public void TestInitialize()
        {

   
            // new Regex(ControlValidationExtensions.BasicPasswordExpression(6));
        }

        private bool ValidatePassword(string value)
        {
            var page = new TestablePage();
            page.EnableEventValidation = false;
            var form = new HtmlForm() { ID = "Form1", Name = "Form1", Action = "POST" };
            page.Controls.Add(form);
            
            var textbox = new TextBox();
            textbox.ID = "Text1";
            textbox.Text = value;
            form.Controls.Add(textbox);
           
            var validator = new ControlValidatorInjector(textbox).BasicPassword(6);
            validator.ID = "Validator1";

            validator.Validate();

            //var sb = new StringBuilder();
            //var sw = new StringWriter(sb);
            //var writer = new HtmlTextWriter(sw);
            //page.RenderControl(writer);
            //var result = sb.ToString();
            
            return validator.IsValid;
        }

        [TestMethod]
        public void BasicPassword_MinimumLengthSpecialChars_IsValid()
        {
            Assert.IsTrue(ValidatePassword("Abc12!"));
            Assert.IsTrue(ValidatePassword("Abc12@"));
            Assert.IsTrue(ValidatePassword("Abc12#"));
            Assert.IsTrue(ValidatePassword("Abc12$"));
            Assert.IsTrue(ValidatePassword("Abc12%"));
            Assert.IsTrue(ValidatePassword("Abc12^"));
            Assert.IsTrue(ValidatePassword("Abc12&"));
            Assert.IsTrue(ValidatePassword("Abc12*"));
            Assert.IsTrue(ValidatePassword("Abc12("));
            Assert.IsTrue(ValidatePassword("Abc12)"));
            Assert.IsTrue(ValidatePassword("Abc12_"));
            Assert.IsTrue(ValidatePassword("Abc12?"));
            Assert.IsTrue(ValidatePassword("Abc12,"));
            Assert.IsTrue(ValidatePassword("Abc12~"));

            Assert.IsFalse(ValidatePassword("Abc12-"));
            Assert.IsFalse(ValidatePassword("Abc12="));
            Assert.IsFalse(ValidatePassword("Abc12+"));
            Assert.IsFalse(ValidatePassword("Abc12;"));
            Assert.IsFalse(ValidatePassword("Abc12:"));
            Assert.IsFalse(ValidatePassword("Abc12'"));
            Assert.IsFalse(ValidatePassword("Abc12\""));
            Assert.IsFalse(ValidatePassword("Abc12|"));
            Assert.IsFalse(ValidatePassword("Abc12\\"));
            Assert.IsFalse(ValidatePassword("Abc12/"));
            Assert.IsFalse(ValidatePassword("Abc12<"));
            Assert.IsFalse(ValidatePassword("Abc12>"));
        }


        [TestMethod]
        public void BasicPassword_ValidToShort_IsNotValid()
        {
            Assert.IsFalse(ValidatePassword("Ab1!"));
            Assert.IsFalse(ValidatePassword("Ab1@"));
            Assert.IsFalse(ValidatePassword("Ab1#"));
            Assert.IsFalse(ValidatePassword("Ab1$"));
            Assert.IsFalse(ValidatePassword("Ab1%"));
            Assert.IsFalse(ValidatePassword("Ab1^"));
            Assert.IsFalse(ValidatePassword("Ab1&"));
            Assert.IsFalse(ValidatePassword("Ab1*"));
            Assert.IsFalse(ValidatePassword("Ab1("));
            Assert.IsFalse(ValidatePassword("Ab1)"));
            Assert.IsFalse(ValidatePassword("Ab1-"));
            Assert.IsFalse(ValidatePassword("Ab1_"));
            Assert.IsFalse(ValidatePassword("Ab1="));
            Assert.IsFalse(ValidatePassword("Ab1+"));
            Assert.IsFalse(ValidatePassword("Ab1;"));
            Assert.IsFalse(ValidatePassword("Ab1:"));
            Assert.IsFalse(ValidatePassword("Ab1'"));
            Assert.IsFalse(ValidatePassword("Ab1\""));
            Assert.IsFalse(ValidatePassword("Ab1|"));
            Assert.IsFalse(ValidatePassword("Ab1\\"));
            Assert.IsFalse(ValidatePassword("Ab1?"));
            Assert.IsFalse(ValidatePassword("Ab1/"));
            Assert.IsFalse(ValidatePassword("Ab1<"));
            Assert.IsFalse(ValidatePassword("Ab1>"));
            Assert.IsFalse(ValidatePassword("Ab1,"));
            Assert.IsFalse(ValidatePassword("Ab1~"));
        }

        [TestMethod]
        public void BasicPassword_ValidNoSpecialChar_IsNotValid()
        {
            Assert.IsFalse(ValidatePassword("Abc123"));
        }
    }
}
