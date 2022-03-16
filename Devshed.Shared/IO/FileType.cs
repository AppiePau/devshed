namespace Devshed.IO
{
    using System;
    using Devshed.Shared;

    /// <summary>
    /// Contains a file type definition for the FileContainer.
    /// </summary>
    [Serializable]
    public sealed class FileType
    {
        /// <summary>
        /// An empty representation of the FileType class,
        /// </summary>
        public readonly static FileType Empty = new FileType();

        /// <summary>
        /// Initiate a new file type object.
        /// </summary>
        public FileType()
        {
            this.Extension = string.Empty;
            this.ContentType = string.Empty;
        }

        /// <summary>
        /// Initiate a new file type object with an extension and content type.
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="contenttype"></param>
        public FileType(string extension, string contenttype)
        {
            Requires.IsNotNullOrEmpty(extension, "extension");
            Requires.IsNotNullOrEmpty(contenttype, "contenttype");
            Requires.StartsWith(extension, ".", "extension");

            this.Extension = extension;
            this.ContentType = contenttype;
        }

        /// <summary>
        /// Returns the file extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Returns the content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Formats a filename to a full qualified file name.
        /// </summary>
        public string FormatName(string name)
        {
            return string.Format("{0}{1}", name, this.Extension).Replace(' ', '_');
        }

        /// <summary>
        /// Gets a code unique to the extension.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Extension.GetHashCode();
        }

        /// <summary>
        /// Compares to another file type for equality.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(FileType))
            {
                return this.ToString().ToUpper() == obj.ToString().ToUpper();
            }

            return false;
        }

        /// <summary>
        /// A string representation of the file type (extension).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Extension;
        }
    }
}