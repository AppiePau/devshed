namespace Devshed.Web
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public static class ControlValidationExtensions
    {
        public static string BasicPasswordExpression(int length)
        {
            return "^.*(?=.{" + length + ",})(?=.*[a-zA-Z])(?=.*\\d)(?=.*[!@#$%^&*?()-_+=\"'\\|?/.,<>]).*$";
        }

        public static string UrlValidationExpression =
                @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";

        public static string EmailValidationExpression =
                @"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z](?:[a-zA-Z-]*[a-zA-Z])?$";

        public static RegularExpressionValidator BasicPassword(this ControlValidatorInjector controlValidator)
        {
            return BasicPassword(controlValidator, null);
        }

        public static RegularExpressionValidator BasicPassword(this ControlValidatorInjector controlValidator, int? length)
        {
            var validator = controlValidator.AddValidator<RegularExpressionValidator>(Resources.Validation.PasswordDoesNotMatchRequirements);
            validator.ValidationExpression = BasicPasswordExpression(length ?? 6);
            return validator;
        }

        public static RegularExpressionValidator AsUrl(this ControlValidatorInjector controlValidator)
        {
            var validator = controlValidator.AddValidator<RegularExpressionValidator>(Resources.Validation.UrlIsNotValid);
            validator.ValidationExpression = UrlValidationExpression;
            return validator;
        }

        public static RegularExpressionValidator AsEmail(this ControlValidatorInjector controlValidator)
        {
            var validator = controlValidator.AddValidator<RegularExpressionValidator>(Resources.Validation.EmailIsNotValid);
            validator.ValidationExpression = EmailValidationExpression;
            return validator;
        }

        public static TControl SetCustomInputMask<TControl>(this TControl control, string inputMask) where TControl : WebControl
        {
            control.Attributes["data-mask"] = inputMask;
            return control;
        }

        /// <summary> Adds a validator to the control and registers it in the page. </summary>
        /// <param name="controlValidator"> The control to validate. </param>
        /// <param name="friendlyName"> Friendly name of the required field.</param>
        public static RequiredFieldValidator RequiredField(this ControlValidatorInjector controlValidator, string friendlyName)
        {
            controlValidator.CssClass += " aspRequired";
            return controlValidator.AddValidator<RequiredFieldValidator>(
                string.Format(Resources.Validation.RequiredField, friendlyName));
        }

        public static CompareValidator CompareToControlValue(
            this ControlValidatorInjector controlValidator,
            WebControl controlToCompare,
            string friendlyName)
        {
            var validator = controlValidator.AddValidator<CompareValidator>(
                string.Format(Resources.Validation.ComparedValueDontMatchField, friendlyName));
            validator.ControlToCompare = controlToCompare.ID;
            validator.Operator = ValidationCompareOperator.Equal;
            validator.Type = ValidationDataType.String;

            return validator;
        }

        /// <summary> Adds a validator to the control and registers it in the page. </summary>
        /// <param name="controlValidator"> The control to be validated. </param>
        /// <param name="controlToCompare"> The control to compare </param>
        /// <param name="compareOperator"> Compare operator. </param>
        /// <param name="type"> The type of comparison. </param>
        /// <returns> The created <see cref="CompareValidator"/> object.</returns>
        public static CompareValidator CompareToControl(
            this ControlValidatorInjector controlValidator,
            WebControl controlToCompare,
            ValidationCompareOperator compareOperator,
            ValidationDataType type)
        {
            var validator = controlValidator.AddValidator<CompareValidator>(Resources.Validation.ComparedControlValuesDontMatch);
            validator.ControlToCompare = controlToCompare.ID;
            validator.Operator = compareOperator;
            validator.Type = type;

            return validator;
        }

        /// <summary> Adds a validator to the control and registers it in the page. </summary>
        /// <param name="controlValidator"> The  control to validate. </param>
        /// <returns> The created <see cref="CompareValidator"/> object. </returns>
        public static CompareValidator AsInteger(this ControlValidatorInjector controlValidator)
        {
            return ValidateAsDataType(controlValidator, ValidationDataType.Integer);
        }

        /// <summary> Adds a validator to the control and registers it in the page. </summary>
        /// <param name="controlValidator"> The  control to validate. </param>
        /// <returns> The created <see cref="CompareValidator"/> object. </returns>
        public static CompareValidator AsDateTime(this ControlValidatorInjector controlValidator)
        {
            controlValidator.CssClass += " date";
            return ValidateAsDataType(controlValidator, ValidationDataType.Date);
        }

        /// <summary> Adds a validator to the control and registers it in the page. </summary>
        /// <param name="controlValidator"> The  control to validate. </param>
        /// <param name="friendlyName"> The friendly name to display in the error. </param>
        /// <returns> The created <see cref="CompareValidator"/> object. </returns>
        public static RegularExpressionValidator AsTime(this ControlValidatorInjector controlValidator, string friendlyName)
        {
            controlValidator.CssClass += " time";
            var validator = controlValidator.AddValidator<RegularExpressionValidator>(
                    string.Format(Resources.Validation.TimeFormatInvalid, friendlyName));
            validator.ValidationExpression = Constants.TimeValidationExpression;
            return validator;
        }

        /// <summary> Adds a validator to the control and registers it in the page. </summary>
        /// <param name="controlValidator"> The  control to validate. </param>
        /// <returns> The created <see cref="CompareValidator"/> object. </returns>
        public static CompareValidator AsCurrency(this ControlValidatorInjector controlValidator)
        {
            return ValidateAsDataType(controlValidator, ValidationDataType.Currency);
        }

        /// <summary> Adds a validator to the control and registers it in the page. </summary>
        /// <param name="controlValidator"> The  control to validate. </param>
        /// <returns> The created <see cref="CompareValidator"/> object. </returns>
        public static CompareValidator AsDouble(this ControlValidatorInjector controlValidator)
        {
            return ValidateAsDataType(controlValidator, ValidationDataType.Double);
        }

        /// <summary> Adds a validator to the control and registers it in the page. </summary>
        /// <param name="controlValidator"> The  control to validate. </param>
        /// <returns> The created <see cref="CompareValidator"/> object. </returns>
        public static CompareValidator AsString(this ControlValidatorInjector controlValidator)
        {
            return ValidateAsDataType(controlValidator, ValidationDataType.String);
        }

        public static CompareValidator IsEarlierThan(this ControlValidatorInjector controlValidator, DateTime date)
        {
            return CreateDateComparer(
                controlValidator,
                string.Format(Resources.Validation.DateMustBeEarlierThan, date.ToString()),
                date,
                ValidationCompareOperator.LessThan);
        }

        public static CompareValidator IsEarlierOrEqual(this ControlValidatorInjector controlValidator, DateTime date)
        {
            return CreateDateComparer(
                controlValidator,
                string.Format(Resources.Validation.DateMustBeEarlierOrEqual, date.ToString()),
                date,
                ValidationCompareOperator.LessThanEqual);
        }

        public static CompareValidator IsEarlierOrEqual(this ControlValidatorInjector controlValidator, Control control, string source, string name)
        {
            return CreateDateComparer(
                controlValidator,
                string.Format(Resources.Validation.DateControlMustBeEarlierOrEqual, source),
                control,
                source,
                name.ToLower(),
                ValidationCompareOperator.LessThanEqual);
        }

        public static CompareValidator IsLaterThan(this ControlValidatorInjector controlValidator, DateTime date)
        {
            return CreateDateComparer(
                controlValidator,
                string.Format(Resources.Validation.DateMustBeLaterThan, date.ToString()),
                date,
                ValidationCompareOperator.GreaterThan);
        }

        public static CompareValidator IsLaterOrEqual(this ControlValidatorInjector controlValidator, DateTime date)
        {
            return CreateDateComparer(
                controlValidator,
                string.Format(Resources.Validation.DateMustBeLaterOrEqual, date.ToString()),
                date,
                ValidationCompareOperator.GreaterThanEqual);
        }

        public static CompareValidator IsGreaterThan(this ControlValidatorInjector controlValidator, int value)
        {
            return IntCompareValidator(
                controlValidator,
                value,
                string.Format(Resources.Validation.NumberMustBeHigherThan, value),
                ValidationCompareOperator.GreaterThan);
        }

        public static CompareValidator IsGreaterOrEqual(this ControlValidatorInjector controlValidator, int value)
        {
            return IntCompareValidator(
                controlValidator,
                value,
                string.Format(Resources.Validation.NumberMustBeHigherOrEqualTo, value),
                ValidationCompareOperator.GreaterThanEqual);
        }

        public static CompareValidator IsLessOrEqual(this ControlValidatorInjector controlValidator, int value)
        {
            return IntCompareValidator(
                controlValidator,
                value,
                string.Format(Resources.Validation.NumberMustBeLessOrEqualTo, value),
                ValidationCompareOperator.LessThanEqual);
        }

        public static CompareValidator IsLessThan(this ControlValidatorInjector controlValidator, int value)
        {
            return IntCompareValidator(
                controlValidator,
                value,
                string.Format(Resources.Validation.NumberMustBeLessThan, value),
                ValidationCompareOperator.LessThan);
        }

        public static RegularExpressionValidator ValidateMaxLength(
            this ControlValidatorInjector controlValidator,
            string friendlyName,
            int maxLength)
        {
            var validator = controlValidator.AddValidator<RegularExpressionValidator>(
                string.Format(Resources.Validation.MaximumStringLengthExceeded, friendlyName, maxLength));
            validator.ValidationExpression = @"^[\s\S]{0," + maxLength.ToString() + "}$";
            return validator;
        }

        public static ConditionalValidator ConditionalCompareField(
            this ControlValidatorInjector controlValidator,
            Control controlToCompare,
            string message)
        {
            var validator = new ConditionalCompareField() { ControlToCompare = controlToCompare.ID };

            controlValidator.AddValidator(validator, message);

            return validator;
        }

        public static ConditionalValidator ConditionalRequiredField(
            this ControlValidatorInjector controlValidator,
            string field)
        {
            var validator = controlValidator.AddValidator<ConditionalRequiredField>(field);

            return validator;
        }

        private static CompareValidator CreateDateComparer(
            ControlValidatorInjector controlValidator,
            string errorMessageFormat,
            DateTime date,
            ValidationCompareOperator compare)
        {
            string errorMessage = string.Format(errorMessageFormat, date);
            var validator = controlValidator.AddValidator<CompareValidator>(errorMessage);
            validator.ValueToCompare = date.ToString("yyyy-MM-dd");
            validator.Operator = compare;
            validator.Type = ValidationDataType.Date;
            return validator;
        }

        private static CompareValidator CreateDateComparer(
          ControlValidatorInjector controlValidator,
          string errorMessageFormat,
          Control control,
          string source,
          string name,
          ValidationCompareOperator compare)
        {
            string errorMessage = string.Format(errorMessageFormat, source, name);
            var validator = controlValidator.AddValidator<CompareValidator>(errorMessage);
            validator.ControlToCompare = control.ID;
            validator.Operator = compare;
            validator.Type = ValidationDataType.Date;
            return validator;
        }

        private static CompareValidator IntCompareValidator(
            ControlValidatorInjector controlValidator,
            int value,
            string errorMessageFormat,
            ValidationCompareOperator equation)
        {
            string errorMessage = string.Format(errorMessageFormat, value);
            var validator = controlValidator.AddValidator<CompareValidator>(errorMessage);
            validator.ValueToCompare = value.ToString(CultureInfo.InvariantCulture);
            validator.Operator = equation;
            validator.Type = ValidationDataType.Double;
            return validator;
        }

        /// <summary> Adds a validator to the control and registers it in the page. </summary>
        /// <param name="controlValidator"> The  control to validate. </param>
        /// <param name="type">The data type to validate.</param>
        /// <returns> The created <see cref="CompareValidator"/> object. </returns>
        private static CompareValidator ValidateAsDataType(ControlValidatorInjector controlValidator, ValidationDataType type)
        {
            var validator = controlValidator.AddValidator<CompareValidator>(
                string.Format(Resources.Validation.ValueMustBeOfType, type));

            validator.Operator = ValidationCompareOperator.DataTypeCheck;
            validator.Type = type;

            return validator;
        }
    }
}
