namespace Devshed.Mvc
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Devshed.Shared;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
         Justification = "They ought to ha ve a strong relation.")]
    public sealed class ExtendedModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Fixes the default model binder's lack to decode enum types when materializing from JSON.
        /// Adds support for array materialization.
        /// </summary>
        protected override object GetPropertyValue(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor,
            IModelBinder propertyBinder)
        {
            var propertyType = propertyDescriptor.PropertyType;          
            if (propertyType.IsArray || propertyType.IsEnum)
            {
                object value = GetCustomConvertedValue(bindingContext, propertyType);

                if (value != null)
                {
                    return value;
                }
            }

            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }

        private static object GetCustomConvertedValue(ModelBindingContext bindingContext, Type propertyType)
        {
            var providerValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);          

            if (providerValue != null && providerValue.AttemptedValue != null)
            {
                return Conversion.AsValue(propertyType, providerValue.AttemptedValue);
            }

            return null;
        }
    }
}
