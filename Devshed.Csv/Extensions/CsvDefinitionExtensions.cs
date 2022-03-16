// namespace Devshed.Csv
// {
//     using System.IO;

//     /// <summary>Provided CsvDefinition extension methods.</summary>
//     public static class CsvDefinitionExtensions
//     {
//         /// <summary>
//         ///  Writes the CSV document into a new stream and returns it.
//         /// </summary>
//         /// <typeparam name="TRow"></typeparam>
//         /// <param name="definition">The definition.</param>
//         /// <param name="rows">The rows.</param>
//         /// <returns></returns>
//         public static MemoryStream CreateStream<TRow>(this CsvDefinition<TRow> definition, TRow[] rows)
//         {
//             return CsvWriter.WriteAsCsv(definition, rows);            
//         }

//         /// <summary>
//         /// Writes the CSV document into the stream.
//         /// </summary>
//         /// <typeparam name="TRow"></typeparam>
//         /// <param name="stream">The stream.</param>
//         /// <param name="definition">The definition.</param>
//         /// <param name="rows">The rows.</param>
//         public static void WriteStream<TRow>(this CsvDefinition<TRow> definition, Stream stream, TRow[] rows)
//         {
//             CsvWriter.WriteAsCsv(stream, definition, rows); 
//         }

//         /// <summary>
//         /// Reads the CSV document back into an array.
//         /// </summary>
//         /// <typeparam name="TRow">The type of the row.</typeparam>
//         /// <param name="definition">The definition.</param>
//         /// <param name="stream">The stream.</param>
//         /// <returns></returns>
//         public static TRow[] ReadStream<TRow>(this CsvDefinition<TRow> definition, Stream stream) where TRow : new()
//         {
//             return definition.ReadAsCsv<TRow>(stream);
//         }
//     }
// }
