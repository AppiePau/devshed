namespace Devshed.Csv.Reading
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Devshed.Shared;

    public sealed class CsvLine<TRow>
    {
        private readonly CsvLine line;

        public CsvLine(CsvLine line, TRow row)
        {
            this.line = line;
            this.Row = row;
        }

        public IEnumerable<string> ErrorMessages
        {
            get
            {
                return this.line.Line.ErrorMessages;
            }
        }

        public TRow Row { get; private set; }
    }

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

            this.reader = new CsvStreamLineReader(definition, headers);
            this.reader.FirstRowContainsHeaders = this.Definition.FirstRowContainsHeaders;
        }

        /// <summary>
        /// Gets the mapped rows.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public IEnumerable<CsvLine<TRow>> GetRows(CsvStreamReader reader)
        {
            foreach (var line in this.reader.GetRows(reader))
            {
                var row = new TRow();

                this.SetPropertyValues(line, row);

                yield return new CsvLine<TRow>(line, row);
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
                            SetPropertyValue(row, column, line, element);
                        }
                        catch (NullReferenceException e)
                        {
                            var message = "The value of '" + header + "' (" + column.PropertyName + ") was NULL on line " + line.Line.LineNumber + ".";
                            line.Line.ErrorMessages.Add(message);

                            if (Definition.ThrowExceptionOnError)
                            {
                                throw new CsvStreamMapperException(message, e, line);
                            }
                        }
                        catch (ArgumentException e)
                        {
                            var message = "The value of '" + header + "' (" + column.PropertyName + ") was invalid on line " + line.Line.LineNumber + ".";
                            line.Line.ErrorMessages.Add(message);

                            if (Definition.ThrowExceptionOnError)
                            {
                                throw new CsvStreamMapperException(message, e, line);
                            }
                        }
                        catch (Exception e)
                        {
                            var message = "An error ocurred in field '" + header + "' (" + column.PropertyName + ") on line " + line.Line.LineNumber + ".";
                            line.Line.ErrorMessages.Add(message);

                            if (Definition.ThrowExceptionOnError)
                            {
                                throw new CsvStreamMapperException(message, e, line);
                            }
                        }
                    }
                }
            }
        }

        private void SetPropertyValue(TRow row, ICsvColumn<TRow> column, CsvLine line, string element)
        {
            PropertyInfo prop = typeof(TRow).GetProperty(column.PropertyName);
            if (!this.Definition.IgnoreReadonlyProperties && !prop.CanWrite)
            {
                throw new CsvStreamMapperException("The field '" + column.PropertyName + "' is readonly (e.g. is not writable).", line);
            }
            if (prop.CanWrite)
            {
                prop.SetValue(row, Conversion.AsValue(prop.PropertyType, element), null);
            }
        }

        private string GetValue(CsvLine line, string header)
        {
            try
            {
                return line[header.ToUpper()];
            }
            catch (KeyNotFoundException exception)
            {
                var message = "The corresponding value of header name '" + header + "' was not found in the collection on line " + line.Line.LineNumber + ".";
                line.Line.ErrorMessages.Add(message);

                if (Definition.ThrowExceptionOnError)
                {
                    throw new CsvStreamMapperException(message, exception, line);
                }
            }

            return null;
        }
    }
}
