namespace Devshed.IO
{
    using Devshed.Shared;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.StaticFiles;

    /// <summary> Looks up the MIME type from the registry. </summary>
    public sealed class StaticFileTypeProvider : IFileTypeProvider
    {
        /// <summary>
        /// Gets the file type of file by extension from the Windows registry.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public FileType GetMimeType(string extension)
        {
            Requires.StartsWith(extension, ".", "extension");

            var provider = new FileExtensionContentTypeProvider();

            string contentType;
            if (!provider.TryGetContentType(extension, out contentType))
            {
                contentType = "application/octet-stream";
            }


            return new FileType(extension, contentType);
        }
    }
}
