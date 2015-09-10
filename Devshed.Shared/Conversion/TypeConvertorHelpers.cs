namespace Devshed.Shared
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;

    internal static class TypeConvertorHelpers
    {
        //// [DebuggerStepThrough]
        internal static TypeConverter GetConverterForType(Type type)
        {
            if (type.IsArray)
            {
                Type arrayElementType = type.GetElementType();

                if (arrayElementType.IsValueType || arrayElementType == typeof(string))
                {
                    // For ValueType arrays (like int[], Guid[] en int?[]) we will use our own converter
                    // because the .NET ArrayConverter cannot conver comma seperated values.
                    Type converterType =
                        typeof(ArrayConverter<>).MakeGenericType(new[] { arrayElementType });

                    return (TypeConverter)Activator.CreateInstance(converterType);
                }
            }

            return TypeDescriptor.GetConverter(type);
        }

        /// <summary>Converts arrays of values.</summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        private sealed class ArrayConverter<T> : TypeConverter
        {
            private const char SplitChar = ',';

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string)
                    || this.CanConvertFrom(context, sourceType);
            }

            [DebuggerStepThrough]
            public override object ConvertTo(
                ITypeDescriptorContext context,
                CultureInfo culture,
                object value,
                Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    if (value == null)
                    {
                        return null;
                    }

                    T[] collection = value as T[];

                    if (collection == null)
                    {
                        throw new ArgumentException("Invalid type " + value.GetType().FullName, "value");
                    }

                    return ConvertArrayToString(context, culture, collection);
                }

                return this.ConvertTo(context, culture, value, destinationType);
            }

            [DebuggerStepThrough]
            public override object ConvertFrom(
                ITypeDescriptorContext context,
                CultureInfo culture,
                object value)
            {
                if (value == null)
                {
                    return null;
                }

                string val = value as string;

                if (val != null)
                {
                    return ConvertStringToArray(context, culture, val);
                }

                return this.ConvertFrom(context, culture, value);
            }

            [DebuggerStepThrough]
            private static string ConvertArrayToString(
                ITypeDescriptorContext context,
                CultureInfo culture,
                T[] collection)
            {
                StringBuilder builder = new StringBuilder();

                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                bool first = true;

                string splitChar = SplitChar.ToString();

                foreach (T t in collection)
                {
                    if (!first)
                    {
                        builder.Append(splitChar);
                    }

                    string convertedValue = converter.ConvertToString(context, culture, t);

                    builder.Append(convertedValue);

                    first = false;
                }

                return builder.ToString();
            }

            [DebuggerStepThrough]
            private static T[] ConvertStringToArray(
                ITypeDescriptorContext context,
                CultureInfo culture,
                string value)
            {
                if (value.Length == 0)
                {
                    // WARNING: The conversion is incorrect when converting an array of a nullable type
                    // with a single element with a value of null (i.e. new int?[1] { null }). This will
                    // convert to an empty string, while converting back would result in an array with no
                    // elements. The only way to solve this in this converter is to denote non-empty
                    // arrays with for instance brackets, but this would break existing code. We therefore
                    // move the responsibility of this to the caller.
                    return new T[0];
                }

                string[] elements = value.Split(new char[] { SplitChar });

                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                T[] list = new T[elements.Length];
                int index = 0;

                foreach (string element in elements)
                {
                    try
                    {
                        list[index] = (T)converter.ConvertFromString(context, culture, element);
                        index++;
                    }
                    catch (FormatException ex)
                    {
                        // Throw a more expressive message.
                        throw new FormatException(ex.Message + " Supplied value: '" + element + "'.", ex);
                    }
                }

                return list;
            }
        }
    }
}
