namespace Devshed.Csv
{
    using System;
    using System.Linq;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using Devshed.Shared;
    using System.Globalization;
    using System.Collections.Generic;
    using Devshed.Csv.Writing;

    /// <summary> Provides basic column behavior. </summary>
    /// <typeparam name="TSource">The selected source row type.</typeparam>
    /// <typeparam name="TResult">The selected result proprty type to be rendered.</typeparam>
    [DebuggerDisplay("{HeaderName}")]
    public abstract class ColumnDefinition<TSource, TResult> : IColumDefinition<TSource>
    {
        protected readonly Expression<Func<TSource, TResult>> propertySelector;

        private HeaderCollection headers;

        private Func<TSource, TResult> _selector;

        /// <summary>
        /// Returns the result type of the data type that is being mapped.
        /// </summary>
        /// <example>
        ///    var column = new TextCsvColumn<T>(m => m.Username);
        ///    column.ConversionResultType == typeof(string); // returns true
        /// </example>
        public Type ConversionResultType
        {
            get
            {
                return typeof(TResult);
            }
        }

        /// <summary>
        /// Gets or sets the name of the header. The reflected property name by default.
        /// </summary>
        /// <value>
        /// The name of the header.
        /// </value>
        public string HeaderName
        {
            get
            {
                return this.headers.Single().Name;
            }
            set
            {
                this.headers = new HeaderCollection(new[] { value });
            }
        }

        public ColumnDefinition(string propertyName)
            : this(propertyName, propertyName)
        {
        }

        public ColumnDefinition(string propertyName, params string[] headers)
        {
            ParameterExpression argParam = Expression.Parameter(typeof(TSource), "s");
            var selectedField = Expression.PropertyOrField(argParam, propertyName);
            this.propertySelector = Expression.Lambda<Func<TSource, TResult>>(selectedField, argParam);

            this.PropertyName = propertyName;
            this.headers = new HeaderCollection(headers);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        public ColumnDefinition(Expression<Func<TSource, TResult>> selector)
            : this(selector, selector.GetMemberName())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="header">The header.</param>
        public ColumnDefinition(Expression<Func<TSource, TResult>> selector, string header)
            : this(selector, new HeaderCollection(new[] { header }))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="selector">The property selector.</param>
        /// <param name="headers">The property headers.</param>
        public ColumnDefinition(Expression<Func<TSource, TResult>> selector, HeaderCollection headers)
        {
            Requires.IsNotNull(selector, "selector");
            Requires.IsNotNull(headers, "headers");

            this.propertySelector = selector;
            this.headers = new HeaderCollection(headers);
            this.PropertyName = selector.GetMemberName();
        }

        /// <summary>
        /// Gets the reflected name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; private set; }

        public abstract ColumnDataType DataType { get; }


        public Func<TSource, TResult> Selector
        {
            get
            {
                if (this._selector == null)
                {
                    this._selector = this.propertySelector.Compile();
                }

                return this._selector;
            }
        }

        /// <summary>
        /// Gets the header names for this column.
        /// </summary>
        /// <param name="rows"> Contains the whole set of data. </param>
        /// <remarks>
        /// Multiple header names are posible for composite columns.
        /// </remarks>
        /// <returns></returns>
        public virtual HeaderCollection GetWritingHeaderNames(TSource[] rows)
        {
            return new HeaderCollection(new[] { this.HeaderName });
        }

        public virtual HeaderCollection GetReadingHeaderNames()
        {
            return new HeaderCollection(new[] { this.HeaderName });
        }

        /// <summary>
        /// Renders the specified element contents.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public virtual IColumnValueProvider[] Render(ICsvDefinition definition, TSource element, CultureInfo formattingCulture, IStringFormatter formatter)
        {
            var value = this.Selector(element);

            return new[] { this.OnRender(definition, value, formattingCulture.Parent, formatter) };
        }

        protected virtual IColumnValueProvider OnRender(ICsvDefinition defintion, TResult value, CultureInfo formattingCulture, IStringFormatter formatter)
        {
            if (value == null)
            {
                return new CsvColumnValue(string.Empty);
            }

            return new CsvColumnValue(value.ToString());
        }
    }
}
