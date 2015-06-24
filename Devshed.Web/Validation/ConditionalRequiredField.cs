namespace Devshed.Web
{
    public sealed class ConditionalRequiredField : ConditionalValidator
    {
        protected override bool EvaluateIsValid()
        {
            string value = this.GetControlValidationValue(this.ControlToValidate);

            if (this.ValidateCondition())
            {
                return !string.IsNullOrEmpty(value);
            }

            return true;
        }
    }
}