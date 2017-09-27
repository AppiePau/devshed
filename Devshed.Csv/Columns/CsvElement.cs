namespace Devshed.Csv
{
    using System;
    using System.Linq;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using Devshed.Shared;

    /// <summary> Provides basic column behavior. </summary>
    /// <typeparam name="TSource">The selected source row type.</typeparam>
    /// <typeparam name="TResult">The selected result proprty type to be rendered.</typeparam>
    [DebuggerDisplay("{HeaderName}")]
    public abstract class CsvColumn<TSource, TResult> : ICsvColumn<TSource>
    {
        private string[] headers;

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
                return this.headers.Single();
            }
            set
            {
                this.headers = new[] { value };
            }
        }

        public CsvColumn(string propertyName)
            : this(propertyName, propertyName)
        {
        }

        public CsvColumn(string propertyName, params string[] headers)
        {
            ParameterExpression argParam = Expression.Parameter(typeof(TSource), "s");
            var selectedField = Expression.PropertyOrField(argParam, propertyName);
            this.Selector = Expression.Lambda<Func<TSource, TResult>>(selectedField, argParam);

            this.PropertyName = propertyName;
            this.headers = headers;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvColumn{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        public CsvColumn(Expression<Func<TSource, TResult>> selector)
            : this(selector, selector.GetMemberName())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvColumn{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="header">The header.</param>
        public CsvColumn(Expression<Func<TSource, TResult>> selector, string header)
            : this(selector, new[] { header })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvColumn{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="selector">The property selector.</param>
        /// <param name="headers">The property headers.</param>
        public CsvColumn(Expression<Func<TSource, TResult>> selector, string[] headers)
        {
            Requires.IsNotNull(selector, "selector");
            Requires.IsNotNull(headers, "headers");

            this.Selector = selector;
            this.headers = headers;
            this.PropertyName = selector.GetMemberName();
        }

        /// <summary>
        /// Gets the reflected name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; private set; }

        public Expression<Func<TSource, TResult>> Selector { get; private set; }

        /// <summary>
        /// Gets the header names for this column.
        /// </summary>
        /// <remarks>
        /// Multiple header names are posible for composite columns.
        /// </remarks>
        /// <returns></returns>
        public virtual string[] GetHeaderNames()
        {
            return new[] { this.HeaderName };
        }

        /// <summary>
        /// Renders the specified element contents.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public virtual string[] Render(CsvDefinition<TSource> definition, TSource element)
        {    
            var value = this.Selector.Compile().Invoke(element);


            //var value = typeof(TSource).GetProperty(PropertyName).GetValue(element, null);

            
            return new[] { this.OnRender(definition, value) };
        }

        protected virtual string OnRender(CsvDefinition<TSource> defintion, TResult value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString();
        }
    }
}
