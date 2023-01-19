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
    public class XlsXBuilder : IDisposable
    {
        private readonly XLWorkbook workbook;

        private XlsXBuilder(XLWorkbook workbook)
        {
            this.workbook = workbook;
        }

        private XlsXBuilder(XlsXBuilder builder)
        {
            this.workbook = builder.workbook;
        }

        public XlsXBuilder AddSheet<T>(CsvDefinition<T> definition, T[] rows, string name)
        {
            var worksheet = this.workbook.AddWorksheet(name);
            var builder = new XlsxStreamWriter(worksheet);
            builder.Write<T>(rows, definition);

            return new XlsXBuilder(this);
        }

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

        public static XlsXBuilder Create()
        {
            return new XlsXBuilder(new XLWorkbook());
        }

    }

    //public class XlsXBuilder : IDisposable
    //{
    //    private readonly XLWorkbook workbook;

    //    public XlsXBuilder(XLWorkbook workbook)
    //    {
    //        this.workbook = workbook;
    //    }

    //    public XlsXBuilder(XlsXBuilder builder, IXLWorksheet sheet)
    //    {
    //        this.workbook = builder.workbook;
    //        this.workbook.AddWorksheet(sheet);
    //    }

    //    public XlsXBuilder AddSheet(IXLWorksheet sheet)
    //    {
    //        return new XlsXBuilder(this, sheet);
    //    }

    //    public void SaveAs(Stream stream, SaveOptions options = null)
    //    {
    //        if (options != null)
    //        {
    //            workbook.SaveAs(stream, options);
    //        }

    //        workbook.SaveAs(stream);

    //        this.Dispose();
    //    }

    //    public void Dispose()
    //    {
    //        this.workbook.Dispose();
    //    }

    //    //public static IDisposable Create()
    //    //{
    //    //    var workbook = new XLWorkbook();

    //    //    var x = new XlsXBuilder(workbook);

    //    //    return new XlsXBuilderx(x);

    //    //}

    //    //private class XlsXBuilderx : IDisposable
    //    //{
    //    //    private XlsXBuilder x;

    //    //    public XlsXBuilderx(XlsXBuilder x)
    //    //    {
    //    //        this.x = x;
    //    //    }

    //    //    public void Dispose()
    //    //    {
    //    //        x.
    //    //    }
    //    //}
    //}
}