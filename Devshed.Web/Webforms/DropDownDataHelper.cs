namespace Devshed.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;
    using Devshed.Shared;

    public static class DropDownDataHelper
    {
        public static void MakeNullable(this ListControl list)
        {
            list.Items.Insert(0, new ListItem("-", string.Empty));
        }

        public static void BindCollection<T>(
            this ListControl list, IEnumerable<T> collection, Func<T, object> valueFormat, Func<T, object> textFormat)
        {
            BindCollection(list, collection, valueFormat, textFormat, new string[] { });
        }

        //// This overload is meant to support default values from a database id's.
        public static void BindCollection<T>(
            this ListControl list, IEnumerable<T> collection, Func<T, object> valueFormat, Func<T, object> textFormat, int[] selectedValues)
        {
            BindCollection(list, collection, valueFormat, textFormat, selectedValues.Select(i => i.ToString()).ToArray());
        }

        //// This overload is meant to support default values from a database id.
        public static void BindCollection<T>(
            this ListControl list, IEnumerable<T> collection, Func<T, object> valueFormat, Func<T, object> textFormat, int? selectedValue)
        {
            BindCollection(list, collection, valueFormat, textFormat, selectedValue.HasValue ? new[] { selectedValue.Value } : new int[] { });
        }

        public static void BindCollection<T>(
            this ListControl list, IEnumerable<T> collection, Func<T, object> valueFormat, Func<T, object> textFormat, string selectedValue)
        {
            DropDownDataHelper.BindCollection(list, collection, valueFormat, textFormat, new[] { selectedValue });
        }

        public static void BindCollection<T>(
           this ListControl list, IEnumerable<T> collection, Func<T, object> valueFormat, Func<T, object> textFormat, string[] selectedValues)
        {
            list.DataSource =
                from item in collection
                select new
                {
                    Text = textFormat(item).ToString(),
                    Value = valueFormat(item).ToString()
                };

            list.DataValueField = "Value";
            list.DataTextField = "Text";
            list.DataBind();

            list.Items.Cast<ListItem>().ForEach(item => item.Selected = selectedValues.Contains(item.Value));
        }

        public static IEnumerable<TValue> SelectedValues<TValue>(this ListControl control)
        {
            return from item in control.Items.Cast<ListItem>()
                   where item.Selected
                   select Conversion.AsValue<TValue>(item.Value);
        }

        public static TValue SelectedValue<TValue>(this ListControl control)
        {
            return Conversion.AsValue<TValue>(control.SelectedValue);
        }

        public static TValue Value<TValue>(this TextBox control)
        {
            return Conversion.AsValue<TValue>(control.Text);
        }
    }

}
