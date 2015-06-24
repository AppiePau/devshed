namespace Devshed.Shared
{
    using System;
    using System.Text;

    public static class StringExtensions
    {
        /// <summary> Returns the string as an UTF8 byte array. </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetUTF8Bytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary> Returns the string as an UTF7 byte array. </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] GetUTF7Bytes(this string value)
        {
            return Encoding.UTF7.GetBytes(value);
        }

        /// <summary> Returns the string as an ASCII byte array. </summary>
        /// <param name="value"> The string value to convert. </param>
        /// <returns></returns>
        public static byte[] GetASCIIBytes(this string value)
        {
            return Encoding.ASCII.GetBytes(value);
        }

        /// <summary> Returns the string as an Unicode byte array. </summary>
        /// <param name="value"> The string value to convert. </param>
        /// <returns></returns>
        public static byte[] GetUnicodeBytes(this string value)
        {
            return Encoding.Unicode.GetBytes(value);
        }

        /// <summary> Returns the string as an UTF32 byte array. </summary>
        /// <param name="value"> The string value to convert. </param>
        /// <returns></returns>
        public static byte[] GetUTF32Bytes(this string value)
        {
            return Encoding.UTF32.GetBytes(value);
        }

        /// <summary> Returns the string as an BigEndianUnicode byte array. </summary>
        /// <param name="value"> The string value to convert. </param>
        /// <returns></returns>
        public static byte[] GetBigEndianUnicodeBytes(this string value)
        {
            return Encoding.BigEndianUnicode.GetBytes(value);
        }

        /// <summary> Cuts of the the end of and string by the specified length. </summary>
        /// <param name="value"> The string value to cut off. </param>
        /// <returns></returns>
        public static string RemoveBegin(this string value, int length)
        {
            return value.Substring(length, value.Length - length);
        }

        /// <summary> Cuts of the the end of and string by the specified end string. </summary>
        /// <param name="value">The string value to cut off.</param>
        /// <param name="end">The end.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string RemoveBegin(this string value, string end)
        {
            return RemoveBegin(value, end, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary> Cuts of the the end of and string by the specified end string. </summary>
        /// <param name="value">The string value to cut off.</param>
        /// <param name="begin">The end.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string RemoveBegin(this string value, string begin, StringComparison comparison)
        {
            if (value.StartsWith(begin, comparison))
            {
                return value.Substring(begin.Length, value.Length - begin.Length);
            }

            return value;
        }

        /// <summary> Cuts of the the end of and string by the specified length. </summary>
        /// <param name="value"> The string value to cut off. </param>
        /// <returns></returns>
        public static string RemoveEnd(this string value, int length)
        {
            return value.Substring(0, value.Length - length);
        }

        /// <summary> Cuts of the the end of and string by the specified end string. </summary>
        /// <param name="value">The string value to cut off.</param>
        /// <param name="end">The end.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string RemoveEnd(this string value, string end)
        {
            return RemoveEnd(value, end, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary> Cuts of the the end of and string by the specified end string. </summary>
        /// <param name="value">The string value to cut off.</param>
        /// <param name="end">The end.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string RemoveEnd(this string value, string end, StringComparison comparison)
        {
            if (value.EndsWith(end, comparison))
            {
                return value.Substring(0, value.Length - end.Length);
            }

            return value;
        }
    }
}