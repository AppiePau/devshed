using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Devshed.Web
{
    public static class GenericDropDownDataHelper
    {
        public static void BindEnum<TEnum>(
        this ListControl list,
        IEnumerable<EnumSelectListItem<TEnum>> collection)
        where TEnum : struct
        {
            BindEnum(list, collection, new TEnum[] { });
        }

        public static void BindEnum<TEnum>(
        this ListControl list,
        IEnumerable<EnumSelectListItem<TEnum>> collection,
        TEnum? selectedValue)
        where TEnum : struct
        {
            BindEnum(list, collection, selectedValue.HasValue ? new TEnum[] { selectedValue.Value } : new TEnum[] { });
        }

        public static void BindEnum<TEnum>(
        this ListControl list,
        IEnumerable<EnumSelectListItem<TEnum>> collection,
        TEnum[] selectedValues)
        where TEnum : struct
        {
            DropDownDataHelper.BindCollection(
                list,
                collection,
                e => e.Value.ToString(),
                e => e.Name,
                selectedValues.Select(v => v.ToString()).ToArray());
        }
    }

    public class EnumSelectListItem<TEnum> where TEnum : struct
    {
        public EnumSelectListItem(TEnum value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public TEnum Value { get; private set; }

        public string Name { get; private set; }
    }
}