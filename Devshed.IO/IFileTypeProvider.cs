namespace Devshed.IO
{
    /// <summary> Service definition to lool up the MIME types for file. </summary>
    public interface IFileTypeProvider
    {
        /// <summary>
        /// Get the mime/contnent type.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        FileType GetMimeType(string extension);
    }
}