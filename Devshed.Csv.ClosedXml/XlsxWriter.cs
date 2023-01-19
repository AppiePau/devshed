namespace Devshed.Csv
{
    using System;
    using System.IO;
    using System.Linq;
    using ClosedXML.Excel;
    using Devshed.Csv.ClosedXml;

    /// <summary>
    /// 
    /// </summary>
    public static class XlsxWriter
    {
        /// <summary>
        /// Writes the CSV data into a new stream and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition"> The definition to write. </param>
        /// <param name="rows"> The rows to write. </param>
        /// <param name="name"> Name of the document, 'Document' by default. </param>
        /// <returns></returns>
        public static MemoryStream WriteAsXlsx<T>(this CsvDefinition<T> definition, T[] rows, string name = "Document")
        {
            var stream = new MemoryStream();
            definition.WriteAsXlsx(rows, stream, name);
            return stream;
        }

        /// <summary>
        /// Writes the CSV data into the stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition"></param>
        /// <param name="stream"></param>
        /// <param name="rows"></param>
        /// <param name="name"> Name of the document, 'Document' by default. </param>
        public static void WriteAsXlsx<T>(this CsvDefinition<T> definition, T[] rows, Stream stream, string name = "Document")
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(name);

                var builder = new XlsxStreamWriter(worksheet);
                builder.Write<T>(rows, definition);

                workbook.SaveAs(stream);
            }
        }
    }


    /// <summary>
    /// Creates a workbook with multiple sheets.
    /// </summary>
    public class XlsxDocumentBuilder : IDisposable
    {
        private readonly XLWorkbook workbook;

        private XlsxDocumentBuilder(XLWorkbook workbook)
        {
            this.workbook = workbook;
        }

        private XlsxDocumentBuilder(XlsxDocumentBuilder builder)
        {
            this.workbook = builder.workbook;
        }

        /// <summary>
        /// Add a sheet to the document builder.
        /// </summary>
        /// <typeparam name="T">The model type of the definition.</typeparam>
        /// <param name="definition">The XlsxDefinition. </param>
        /// <param name="rows">The array of model entities.</param>
        /// <param name="name">The name of the sheet.</param>
        /// <returns></returns>
        public XlsxDocumentBuilder AddSheet<T>(CsvDefinition<T> definition, T[] rows, string name)
        {
            var worksheet = this.workbook.AddWorksheet(name);
            var builder = new XlsxStreamWriter(worksheet);
            builder.Write<T>(rows, definition);

            return new XlsxDocumentBuilder(this);
        }

        /// <summary>
        /// Save the XLSX file to a stream.
        /// </summary>
        /// <param name="stream">the stream to write to.</param>
        /// <param name="options">Saving options.</param>
        public void SaveAs(Stream stream, SaveOptions options = null)
        {
            if (!this.workbook.Worksheets.Any())
                new InvalidOperationException("There are no sheets defined to write.");

            if (options != null)
            {
                workbook.SaveAs(stream, options);
            }

            workbook.SaveAs(stream);

            this.Dispose();
        }

        /// <summary>
        /// Creates and returns a stream with the XLSX. This closes (disposes) the document.
        /// </summary>
        /// <returns>A stream.</returns>
        public Stream CreateStream()
        {
            var stream = new MemoryStream();
            this.SaveAs(stream);
            stream.Flush();
            stream.Position = 0;
            return stream;
        }


        public void Dispose()
        {
            this.workbook.Dispose();
        }

        public static XlsxDocumentBuilder Create()
        {
            return new XlsxDocumentBuilder(new XLWorkbook());
        }
    }
}