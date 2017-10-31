using System;
namespace Devshed.Web
{
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;


    public sealed class PasswordStrengthValidator : CustomValidator
    {
        private readonly string GenericScript = @"
var BasicPasswordValidation =
{
    Messages: 
    {
        PasswordTooShort: """ + Resources.Validation.PasswordTooShort + @""",
        DoesNotContainDigit: """ + Resources.Validation.DoesNotContainDigit + @""",
        DoesNotContainLetters: """ + Resources.Validation.DoesNotContainLetters + @""",
        DoesNotContainSpecialCharacter: """ + Resources.Validation.DoesNotContainSpecialCharacter + @"""
    },
    CreatePassworScore: function (password, minLength)
    {
	    minLength = minLength || 8;
        var result = { score: 0, messages: [] };
    

        if (password.length < 1)
        
            return result;

        if (password.length < 4)
        {
            result.messages.push(BasicPasswordValidation.Messages.PasswordTooShort);
            return result;
        }

        if (password.Length >= minLength)
        {
            result.score++;
        }

        if (password.Length >= minLength + 4)
        {
            result.score++;
        }

        if ((/\d+/).test(password))
        {
            result.score++;
        }
        else
        {
            result.messages.push(BasicPasswordValidation.Messages.DoesNotContainDigit);
        }

        if ((/[a-z]/).test(password) && (/[A-Z]/).test(password))
        {
            result.score++;
        }
        else
        {
            result.messages.push(BasicPasswordValidation.Messages.DoesNotContainLetters);
        }

        if ((/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/).test(password))
        {
            result.score++;
        }
        else
        {
            result.messages.push(BasicPasswordValidation.Messages.DoesNotContainSpecialCharacter);
        }
 
        return result;
    }
};";

        [Bindable(true)]
        public int MinimumLength
        {
            get { return ViewState["MinimumLength"] as int? ?? 8; }
            set { ViewState["MinimumLength"] = value; }
        }

        [Bindable(true)]
        public PasswordScore MinimumScore
        {
            get { return ViewState["MinimumScore"] as PasswordScore? ?? PasswordScore.Strong; }
            set { ViewState["MinimumScore"] = value; }
        }

        protected override bool OnServerValidate(string value)
        {
            return PasswordAdvisor.CheckStrength(value, MinimumLength).Score >= MinimumScore;
        }

        protected override void OnPreRender(EventArgs e)
        {
            var functionName = "ValidateBasicPassword_" + this.ClientID;

            var script = @"
                function " + functionName + @"(source, arguments)
                {
                    var result = BasicPasswordValidation.CreatePassworScore(arguments.Value); 
                    arguments.IsValid = result.score >= " + (int)MinimumScore + @";

                    var message = '';
                    for(t = 0; t < result.messages.length; t++)
                    {
                        message += ' - ' + result.messages[t] + '\n';
                    }

                    console.log(result);
                    alert(message);
                }";

            this.ClientValidationFunction = functionName;
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), this.UniqueID + "PasswordValidatorScript", script, true);
            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "GenericPasswordValidatorScript", GenericScript, true);
            base.OnPreRender(e);
        }
    }

}