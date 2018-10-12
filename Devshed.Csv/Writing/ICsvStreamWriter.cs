using System.IO;

namespace Devshed.Csv.Writing
{
    public interface ICsvStreamWriter
    {
        void Write<T>(Stream stream, T[] rows, CsvDefinition<T> definition);
    }
}