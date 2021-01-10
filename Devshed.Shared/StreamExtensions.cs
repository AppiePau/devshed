namespace Devshed.Shared
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Stream helper extensions
    /// </summary>
    public static class StreamExtensions
    {

        /// <summary>
        /// Returns the content of the stream as a byte array and removes the BOM (UTF Bit Order Marker).
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="resetPosition">Set the stream position to 0 before reading.</param>
        /// <returns></returns>
        public static byte[] GetBytesWithoutBom(this Stream stream, bool resetPosition = true)
        {
            return GetBytes(stream, removeBom: true, resetPosition: resetPosition);
        }

        /// <summary>
        /// Returns the content of the stream as a byte array hence removing the Bit Order Marker (BOM).
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="removeBom">if set to <c>true</c> remove BOM header.</param>
        /// <param name="resetPosition">if set to <c>true</c> reset the stream.Position to 0.</param>
        /// <returns></returns>
        public static byte[] GetBytes(this Stream stream, bool removeBom = false, bool resetPosition = true)
        {
            if (resetPosition)
            {
                stream.Position = 0;
            }

            if (removeBom)
            {
                stream.Position = BomRemover.GetCursorPosition(stream);
            }

            var buffer = new byte[stream.Length];

            stream.Read(buffer, 0, (int)stream.Length);

            return buffer;
        }

        /// <summary>
        /// Gets the string data from the stream at the beginning.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns></returns>
        public static string GetString(this Stream stream)
        {
            stream.Position = 0;

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
