namespace Devshed.Web
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using Devshed.Shared;

    public static partial class RequestDeserializer
    {
        /// <summary>
        /// Materializes the specified type of object from the post variables. By default the parameters will be treated as culture invariant.
        /// </summary>
        /// <typeparam name="TParameters">The type of object to create.</typeparam>
        /// <returns>
        /// A new instance of the materialized object.
        /// </returns>
        public static TParameters FromPost<TParameters>() where TParameters : new()
        {
            return FromPost<TParameters>(CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Materializes the specified type of object from the post variables. By default the parameters will be treated as culture invariant.
        /// </summary>
        /// <typeparam name="TParameters">The type of object to create.</typeparam>
        /// <param name="culture">The culture formatting of the form variables.</param>
        /// <returns>
        /// A new instance of the materialized object.
        /// </returns>
        public static TParameters FromPost<TParameters>(CultureInfo culture) where TParameters : new()
        {
            return InternalDeserialize<TParameters>(HttpContext.Current.Request.Form, culture);
        }

        /// <summary>
        /// Materializes the specified type of object from the query variables. By default the parameters will be treated as culture invariant.
        /// </summary>
        /// <typeparam name="TParameters">The type of object to create.</typeparam>
        /// <returns>
        /// A new instance of the materialized object.
        /// </returns>
        public static TParameters FromQuery<TParameters>() where TParameters : new()
        {
            return FromQuery<TParameters>(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Materializes the specified type of object from the query variables. By default the parameters will be treated as culture invariant.
        /// </summary>
        /// <typeparam name="TParameters">The type of object to create.</typeparam>
        /// <param name="culture">The culture formatting of the form variables.</param>
        /// <returns>
        /// A new instance of the materialized object.
        /// </returns>
        public static TParameters FromQuery<TParameters>(CultureInfo culture) where TParameters : new()
        {
            return InternalDeserialize<TParameters>(HttpContext.Current.Request.QueryString, culture);
        }

        internal static TParameters InternalDeserialize<TParameters>(NameValueCollection collection, CultureInfo culture) where TParameters : new()
        {
            var parameters = new TParameters();
            InternalDeserialize<TParameters>(parameters, collection, culture);

            return parameters;
        }

        internal static void InternalDeserialize<TParameters>(TParameters parameters, NameValueCollection collection, CultureInfo culture)
        {
            var serializableProperties =
            typeof(TParameters).GetProperties()
            .Where(p => IsSerializableProperty(p.PropertyType));

            foreach (var property in serializableProperties)
            {
                var name = GetPropertyName(property);

                if (collection.AllKeys.Select(e => e.ToUpper()).Contains(name.ToUpper()))
                {
                    string requestValue = collection[name];

                    try
                    {
                        object value = Conversion.AsValue(
                                            property.PropertyType,
                                            requestValue,
                                            culture);

                        property.SetValue(parameters, value, null);
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException(
                        "Error while converting parameter '" + property.Name + "' and value '" + requestValue + "'.", e);
                    }
                }
            }
        }

        private static string GetPropertyName(PropertyInfo property)
        {
            return property.Name;
        }

        [DebuggerStepThrough]
        private static TypeConverter GetConverterForType(Type type)
        {
            if (type.IsArray)
            {
                Type arrayElementType = type.GetElementType();

                if (arrayElementType.IsValueType)
                {
                    // Voor ValueType arrays (zoals int[], Guid[] en int?[]) gebruiken we onze eigen converter
                    // omdat de .NET ArrayConverter comma separated strings niet kan converteren.
                    Type converterType =
                    typeof(ArrayConverter<>).MakeGenericType(new[] { arrayElementType });

                    return (TypeConverter)Activator.CreateInstance(converterType);
                }
            }

            return TypeDescriptor.GetConverter(type);
        }

        private static void ValidateSerializableObject(object param)
        {
            if (!param.GetType().IsClass && !param.GetType().IsAnsiClass)
            {
                throw new InvalidOperationException(
                "The parameter object is not a class, which is expected for serialization.");
            }
        }

        internal static bool IsSerializableProperty(Type type)
        {
            return
            type.IsValueType
            || type == typeof(string)
            || (type.IsArray && type.GetElementType().IsValueType);
        }

        /// <summary>Converts arrays of values.</summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        private sealed class ArrayConverter<T> : TypeConverter
        {
            private static readonly char SplitChar = ',';

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) ||
                base.CanConvertFrom(context, sourceType);
            }

            [DebuggerStepThrough]
            public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
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

                return base.ConvertTo(context, culture, value, destinationType);
            }

            [DebuggerStepThrough]
            public override object ConvertFrom(
            ITypeDescriptorContext context, CultureInfo culture, object value)
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

                return base.ConvertFrom(context, culture, value);
            }

            [DebuggerStepThrough]
            private static string ConvertArrayToString(
            ITypeDescriptorContext context, CultureInfo culture, T[] collection)
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
            ITypeDescriptorContext context, CultureInfo culture, string value)
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