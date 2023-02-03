//namespace Devshed.Csv
//{
//    using System;
//    using System.Linq;
//    using System.Collections.Generic;
//    using System.Linq.Expressions;
//    using Devshed.Csv.Writing;
//    using Devshed.Shared;
//    using System.Globalization;

//    /// <summary>
//    /// Represents an array based CSV column.
//    /// </summary>
//    /// <typeparam name="TSource"> The source of the mapping. </typeparam>
//    /// <typeparam name="TArray"> The type of elements in the array. </typeparam>
//    public class ArrayCsvColumn<TSource, TArray> : CsvColumn<TSource, IEnumerable<TArray>>
//    {
//        private string elementDelimiter;

//        /// <summary>
//        /// Initialize the array column definition.
//        /// </summary>
//        /// <param name="propertyName"> The name of the property bound to. </param>
//        public ArrayCsvColumn(string propertyName)
//            : base(propertyName)
//        {
//        }

//        /// <summary>
//        /// Initialize the array column definition.
//        /// </summary>
//        /// <param name="selector"> The expression mapping the property. </param>
//        public ArrayCsvColumn(Expression<Func<TSource, IEnumerable<TArray>>> selector)
//            : base(selector)
//        {
//            this.ElementDelimiter = ",";
//            this.Format = (value, cult) =>
//            {
//                var values = value.Select(e => e.ToString().Replace(this.ElementDelimiter, "_")).ToArray();

//                return string.Join(this.ElementDelimiter, values);
//            };
//        }

//        /// <summary>
//        /// The data type of the column.
//        /// </summary>
//        public override ColumnDataType DataType
//        {
//            get
//            {
//                return ColumnDataType.Currency;
//            }
//        }

//        /// <summary> Specifies the delimiter between the array element, a comma by default.
//        /// Do not change unless needed, a comma is always needed for reading. </summary>
//        public string ElementDelimiter
//        {
//            get
//            {
//                return this.elementDelimiter;
//            }
//            set
//            {
//                Requires.IsNotNull("ElementDelimiter", value);
//                this.elementDelimiter = value;
//            }
//        }

//        /// <summary>
//        /// The formatting function for rendering the value.
//        /// </summary>
//        public override Func<IEnumerable<TArray>, CultureInfo, string> Format { get; set; }

//        /// <summary>
//        /// Renders the value of the column.
//        /// </summary>
//        /// <param name="defintion"> The CSV definition. </param>
//        /// <param name="value"> The value to render. </param>
//        /// <returns>A string that can be directly written into the CSV file. </returns>
//        protected override object OnRender(ICsvDefinition defintion, IEnumerable<TArray> value)
//        {


//            return element;
//            return formatter.FormatStringCell(element, defintion.RemoveNewLineCharacters);

//        }
//    }
//}
