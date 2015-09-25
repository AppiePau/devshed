namespace Devshed.Csv.Reading
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Devshed.Shared;

    /// <summary> Maps a CSV stream source to a strong typed definition. </summary>
    /// <typeparam name="TRow">The type of the row.</typeparam>
    public sealed class CsvStreamMapper<TRow> where TRow : new()
    {
        private readonly CsvStreamLineReader reader;

        internal CsvDefinition<TRow> Definition { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvStreamMapper{TRow}"/> class.
        /// </summary>
        /// <param name="definition">The document definition.</param>
        public CsvStreamMapper(CsvDefinition<TRow> definition)
        {
            this.Definition = definition;
            var headers = definition.Columns.SelectMany(e => e.GetHeaderNames()).ToArray();

            this.reader = new CsvStreamLineReader(definition.ElementProcessing, headers);
            this.reader.FirstRowContainsHeaders = this.Definition.FirstRowContainsHeaders;
        }

        /// <summary>
        /// Gets the mapped rows.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public IEnumerable<TRow> GetRows(CsvStreamReader reader)
        {            
            foreach (var line in this.reader.GetRows(reader))
            {
                var row = new TRow();

                this.SetPropertyValues(line, row);

                yield return row;
            }
        }

        private void SetPropertyValues(CsvLine line, TRow row)
        {           
            foreach (var column in this.Definition.Columns)
            {
                ////TODO: Add support for composite columns?
                if (column.GetHeaderNames().Count() == 1)
                {
                    foreach (var header in column.GetHeaderNames())
                    {
                        var element = GetValue(line, header);

                        try
                        {
                            SetPropertyValue(row, column, element);
                        }
                        catch (NullReferenceException e)
                        {
                            throw new NullReferenceException("The value of '" + header + "' was NULL on line " + line.LineNumber + ".", e);
                        }
                        catch (ArgumentException e)
                        {
                            throw new ArgumentException("The value of '" + header + "' was invalid on line " + line.LineNumber + ".", e);
                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException("An error ocurred in field '" + header + "' on line " + line.LineNumber + ".", e);
                        }
                    }
                }
            }
        }

        private static void SetPropertyValue(TRow row, ICsvColumn<TRow> column, string element)
        {
            var targetType = typeof(TRow);

            var prop = targetType.GetProperty(column.PropertyName);
            prop.SetValue(row, Conversion.AsValue(prop.PropertyType, element), null);
        }

        private static string GetValue(CsvLine line, string header)
        {
            try
            {
                return line[header.ToUpper()];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException(
                    "Key '" + header + "' was not found in the collection on line " + line.LineNumber + ".", e);
            }
        }
    }
}
