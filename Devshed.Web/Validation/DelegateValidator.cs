namespace Devshed.Web
{
    using System;

    public sealed class DelegateValidator : ConditionalValidator
    {
        public DelegateValidator()
        {
        }

        public DelegateValidator(Func<bool> condition)
        {
            this.Condition = condition;
        }

        public Func<bool> Condition { get; set; }

        protected override bool EvaluateIsValid()
        {
            return this.Condition();
        }
    }
}