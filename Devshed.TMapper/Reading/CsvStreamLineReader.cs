namespace Devshed.Csv.Reading
{
    using System;
    using System.Collections.Generic;
    using System.Linq;


    /// <summary> Helps reading CSV with header names. </summary>
    public sealed class CsvStreamLineReader
    {
        private HeaderCollection headers;

        private readonly ICsvDefinition definition;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvStreamLineReader"/> class.
        /// </summary>
        public CsvStreamLineReader(ICsvDefinition definition)
            : this(definition, new string[] { })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvStreamLineReader"/> class.
        /// </summary>
        /// <param name="headers">The header names.</param>
        public CsvStreamLineReader(ICsvDefinition definition, params string[] headers)
            : this(definition, new HeaderCollection(headers))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvStreamLineReader"/> class.
        /// </summary>
        /// <param name="headerNames">The header names.</param>
        public CsvStreamLineReader(ICsvDefinition definition, HeaderCollection headers)
        {
            this.headers = headers;
            this.definition = definition;
        }

        /// <summary>
        /// Gets or sets a value indicating whether first row contains header names.
        /// </summary>
        /// <value>
        /// <c>true</c> if first row contains header names; otherwise, <c>false</c>.
        /// </value>
        public bool FirstRowContainsHeaders { get; set; }

        /// <summary>
        /// Gets the rows from the CSV stream reader as indexed dictionaries.
        /// </summary>
        /// <param name="reader">The CSV reader.</param>
        /// <returns></returns>
        public IEnumerable<CsvLine> GetRows(IStreamReader reader)
        {
            if (!this.FirstRowContainsHeaders && this.headers.Count() == 0)
            {
                throw new InvalidOperationException(
                    "FirstRowContainsHeaders is set False while headers are empty. Headers are required for indexing.");
            }

            var isFirstTime = true;

            while (!reader.EndOfStream)
            {
                if (this.FirstRowContainsHeaders && isFirstTime)
                {
                    this.headers = new HeaderCollection(reader.ReadLine().Elements);
                    isFirstTime = false;
                }
                else
                {
                    var line = reader.ReadLine();
                    if (!line.IsEmpty)
                    {
                        if (definition.ElementProcessing == ElementProcessing.Strict)
                        {
                            this.ValidateTooManyColumns(line);
                            this.ValidateTooLessColumns(line);
                        }
                        else if (definition.ElementProcessing == ElementProcessing.OnlyTooFew)
                        {
                            this.ValidateTooLessColumns(line);
                        }
                        else if (definition.ElementProcessing == ElementProcessing.OnlyTooMany)
                        {
                            this.ValidateTooManyColumns(line);
                        }

                        yield return this.CreateDictionaryLine(line);
                    }
                }
            }
        }

        private void ValidateTooLessColumns(CsvSourceLine line)
        {
            if (line.Count < this.headers.Count())
            {
                var message = "The line (" + line.LineNumber + ") contains not enough elements (" + line.Count + ") than headers (" + this.headers.Count() + ") available.";
                line.ErrorMessages.Add(message);

                if (definition.ThrowExceptionOnError)
                {
                    throw new InvalidOperationException(message);
                }
            }
        }

        private void ValidateTooManyColumns(CsvSourceLine line)
        {
            if (line.Count > this.headers.Count())
            {
                var message = "The line (" + line.LineNumber + ") contains more elements (" + line.Count + ") than headers (" + this.headers.Count() + ") available.";
                line.ErrorMessages.Add(message);

                if (definition.ThrowExceptionOnError)
                {
                    throw new InvalidOperationException(message);
                }
            }
        }

        private CsvLine CreateDictionaryLine(CsvSourceLine line)
        {
            var indexedLine = new CsvLine(line);
            for (int index = 0; index < this.headers.Length; index++)
            {
                var header = this.headers.GetHeaderName(index);

                if (indexedLine.ContainsKey(header))
                {
                    line.ErrorMessages.Add($"A duplicate header name was found '{header}' on line {line.LineNumber}.");

                    if (definition.ThrowExceptionOnError)
                    {
                        throw new DuplicateHeaderException(header.Name, line.LineNumber);
                    }
                }

                indexedLine.Add(header, GetElementValue(line, index));
            }

            return indexedLine;
        }

        private static string GetElementValue(CsvSourceLine line, int index)
        {
            if (index >= line.Elements.Length)
            {
                return string.Empty;
            }

            return line.Elements[index];
        }
    }
}
