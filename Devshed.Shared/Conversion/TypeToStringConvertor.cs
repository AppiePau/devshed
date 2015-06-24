namespace Devshed.Shared
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;

    public class TypeToStringConvertor
    {
        [DebuggerStepThrough]
        public virtual string AsString(Type type, object value, CultureInfo culture)
        {
            TypeConverter converter = TypeConvertorHelpers.GetConverterForType(type);

            return converter.ConvertToString(null, culture, value);
        }
    }
}
