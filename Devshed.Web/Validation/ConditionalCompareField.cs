namespace Devshed.Web
{
    public sealed class ConditionalCompareField : ConditionalValidator
    {
        public string ControlToCompare { get; set; }

        protected override bool EvaluateIsValid()
        {
            string value = this.GetControlValidationValue(this.ControlToValidate);
            string compareValue = this.GetControlValidationValue(this.ControlToCompare);

            if (value != compareValue)
            {
                return false;
            }

            return true;
        }
    }
}