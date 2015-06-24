namespace Devshed.Web
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ControlValidatorInjector
    {
        private readonly Control control;

        public ControlValidatorInjector(Control control)
        {
            this.control = control;
        }

        public string CssClass
        {
            get
            {
                string classAttribute = ((IAttributeAccessor)this.control).GetAttribute("class");

                if (this.IsWebControl())
                {
                    return ((WebControl)this.control).CssClass + " " + classAttribute;
                }

                return this.CleanupClass(classAttribute);
            }

            set
            {
                var classAttribute = this.CleanupClass(value);

                if (this.IsWebControl())
                {
                    ((WebControl)this.control).CssClass = classAttribute;

                    var classValue = ((IAttributeAccessor)this.control).GetAttribute("class");
                    if (classValue != null)
                    {
                        ((WebControl)this.control).Attributes.Remove("class");
                    }
                }
                else
                {
                    ((IAttributeAccessor)this.control).SetAttribute("class", classAttribute);
                }
            }
        }

        /// <summary>Creates and adds a specified validator to the page to validate a control. </summary>
        /// <typeparam name="TValidator"></typeparam>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public TValidator AddValidator<TValidator>(string errorMessage) where TValidator : BaseValidator, new()
        {
            return this.AddValidator(new TValidator(), errorMessage);
        }

        public TValidator AddValidator<TValidator>(TValidator validator, string errorMessage) where TValidator : BaseValidator
        {
            validator.ErrorMessage = errorMessage;
            validator.Text = errorMessage;
            validator.ToolTip = errorMessage;

            this.AddValidator(validator);
            return validator;
        }

        public TValidator AddValidator<TValidator>(TValidator validator) where TValidator : BaseValidator
        {
            validator.Display = ValidatorDisplay.Dynamic;
            validator.ControlToValidate = this.control.ID;
            validator.CssClass = "ValidatorError";

            this.AddValidatorToPageAfterControl(validator);
            return validator;
        }

        /// <summary> Gets the position before the control to validate. </summary>
        /// <param name="control">The control to be validated.</param>
        /// <returns></returns>
        private static int GetPositionBeforeControl(WebControl control)
        {
            // NOTE: Indexed value must be increased by 1.
            return control.Parent.Controls.IndexOf(control) + 1;
        }

        private string CleanupClass(string classAttribute)
        {
            return Regex.Replace(classAttribute, " {2,}", " ").Trim(' ');
        }

        private bool IsWebControl()
        {
            return this.control.GetType().IsSubclassOf(typeof(WebControl));
        }

        /// <summary> Adds the validator to page after control to validate. </summary>
        /// <param name="validator">The validator.</param>
        private void AddValidatorToPageAfterControl(BaseValidator validator)
        {
            var parent = this.control.Parent;
            int indexOfControlInParent = parent.Controls.IndexOf(this.control);

            try
            {
                parent.Controls.AddAt(indexOfControlInParent + 1, validator);
            }
            catch (HttpException ex)
            {
                string messageFormat =
                    "Sorry, you have encountered a rare .NET bug. " 
                    + "The list of controls that contains the  '{0}', does also contain '<% %>' tags. " 
                    + "In that case the control collection in '{1}' cannot be modified. " 
                    + "To solve this problem, you could put the '{0}' control in another parent. {2}";

                string message = string.Format(
                        CultureInfo.InvariantCulture,
                        messageFormat,
                        this.control.ID,
                        parent.ID,
                        ex.Message);

                throw new InvalidOperationException(message);
            }
        }
    }
}