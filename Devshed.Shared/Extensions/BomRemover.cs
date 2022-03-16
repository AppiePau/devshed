namespace Devshed.Shared
{
    using System.IO;
    using System.Linq;

    /// <summary> Helps removing the Bit Order Mark (BOM). </summary>
    public static class BomRemover
    {
        /// <summary>
        /// Gets the cursor position in the stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static int GetCursorPosition(Stream stream)
        {
            //// UTF-32, big-endian
            if (IsMatch(stream, new byte[] { 0x00, 0x00, 0xFE, 0xFF }))
            {
                return 4;
            }
            //// UTF-32, little-endian
            if (IsMatch(stream, new byte[] { 0xFF, 0xFE, 0x00, 0x00 }))
            {
                return 4;
            }
            //// UTF-16, big-endian
            if (IsMatch(stream, new byte[] { 0xFE, 0xFF }))
            {
                return 2;
            }
            //// UTF-16, little-endian
            if (IsMatch(stream, new byte[] { 0xFF, 0xFE }))
            {
                return 2;
            }
            //// UTF-8
            if (IsMatch(stream, new byte[] { 0xEF, 0xBB, 0xBF }))
            {
                return 3;
            }

            return 0;
        }

        private static bool IsMatch(Stream stream, byte[] match)
        {
            stream.Position = 0;
            var buffer = new byte[match.Length];
            stream.Read(buffer, 0, buffer.Length);
            return !buffer.Where((t, i) => t != match[i]).Any();
        }
    }
}
