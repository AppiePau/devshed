using System.Collections.Generic;

namespace Devshed.Csv.Reading
{
    /// <summary>
    ///   <para>Represents the CSV stream reader.</para>
    /// </summary>
    public interface IStreamReader
    {
        /// <summary>Gets a value indicating whether the stream has reached the end.</summary>
        /// <value>
        ///   <c>true</c> if [end of stream]; otherwise, <c>false</c>.</value>
        bool EndOfStream { get; }

        /// <summary>Reads the CSV line.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        CsvSourceLine ReadLine();
    }
}