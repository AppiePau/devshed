namespace Devshed.IO
{
    using Devshed.Shared;
    using Microsoft.Win32;

    /// <summary> Looks up the MIME type from the registry. </summary>
    public sealed class RegistryFileTypeProvider : IFileTypeProvider
    {
        /// <summary>
        /// Gets the file type of file by extension from the Windows registry.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public FileType GetMimeType(string extension)
        {
            Requires.StartsWith(extension, ".", "extension");

            var registryKey = this.GetExtensionKey(extension);

            if (registryKey != null && registryKey.GetValue("Content Type") != null)
            {
                return new FileType(extension, registryKey.GetValue("Content Type").ToString());
            }

            return new FileType(extension, "application/octet-stream");
        }

        private RegistryKey GetExtensionKey(string extension)
        {
            return Registry.ClassesRoot.OpenSubKey(extension);
        }
    }
}
