using System.Collections.Generic;

namespace Devshed.Csv.Reading
{
    public interface IStreamReader
    {
        bool EndOfStream { get; }

        CsvSourceLine ReadLine();
    }
}