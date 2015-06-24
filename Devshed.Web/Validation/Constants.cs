namespace Devshed.Web
{
    public static class Constants
    {
        public const string CurrencyValidationExpression = "[-+]?([0-9]{0,3}(.[0-9]{3})*,?[0-9]+)";
        public const string DecimalValidationExpression = "[-+]?([0-9]{0,3}(.[0-9]{3})*,?[0-9]+)";
        public const string NumberValidationExpression = "[0-9]*";
        public const string PostalCodeValidationExpression = "[0-9]{4}\\s[A-Z]{2}";
        public const string EmailValidationExpression = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])";
        public const string TimeValidationExpression = "(2[0-3]|[0-1][0-9]):[0-5][0-9]";

        public const string DutchShortDateFormat = "dd-MM-yyyy";
        public const string DutchShortTimeFormat = "HH:mm";

        public const int NewItemId = 0;
    }
}