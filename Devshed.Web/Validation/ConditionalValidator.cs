namespace Devshed.Web
{
    using System;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public abstract class ConditionalValidator : BaseValidator, IValidator
    {
        public Func<bool> ValidateCondition { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var sb = new StringBuilder();
            sb.AppendLine("function ToggleCondtionalValidators(enabled) {");
            sb.AppendLine("    $(ConditionalRequiredFieldValidators).each(function (key, validator) {");
            sb.AppendLine("        var element = $(validator)[0];");
            sb.AppendLine("        if(element != null) { ValidatorEnable(element, enabled); }");
            sb.AppendLine("    });");
            sb.AppendLine("}");

            this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "ConditionalRequiredFieldValidators", sb.ToString(), true);
            this.Page.ClientScript.RegisterArrayDeclaration("ConditionalRequiredFieldValidators", "'" + this.ClientID + "'");
        }
    }

}