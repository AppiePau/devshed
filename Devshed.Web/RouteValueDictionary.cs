namespace Devshed.Web
{
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System;
    using Devshed.Shared;

    public class ParameterValueDictionary : Dictionary<string, string>
    {
        public ParameterValueDictionary()
        {
        }

        public ParameterValueDictionary(object parameters)
        {
            Requires.IsNotNull(parameters, "parameters");

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(parameters))
            {
                if (RequestDeserializer.IsSerializableProperty(descriptor.PropertyType))
                {
                    object paramValue = descriptor.GetValue(parameters);

                    if (paramValue != null)
                    {
                        string value = Conversion.AsString(descriptor.PropertyType, paramValue, CultureInfo.InvariantCulture);
                        this.Add(GetName(descriptor), value);
                    }
                }
            }
        }

        private static string GetName(PropertyDescriptor descriptor)
        {
            return descriptor.Name;
        }

        public void Merge(ParameterValueDictionary source)
        {
            foreach (var param in source)
            {
                this[param.Key] = param.Value;
            }
        }

        private object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }
            else
            {
                return null;
            }
        }
    }
}

