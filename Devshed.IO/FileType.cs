namespace Devshed.IO
{
    using System;
    using Devshed.Shared;

    [Serializable]
    public sealed class FileType
    {
        public readonly static FileType Empty = new FileType();

        public FileType()
        {
            this.Extension = string.Empty;
            this.ContentType = string.Empty;
        }

        public FileType(string extension, string contenttype)
        {
            Requires.IsNotNullOrEmpty(extension, "extension");
            Requires.IsNotNullOrEmpty(contenttype, "contenttype");
            Requires.StartsWith(extension, ".", "extension");

            this.Extension = extension;
            this.ContentType = contenttype;
        }

        public string Extension { get; set; }

        public string ContentType { get; set; }

        public string FormatName(string name)
        {
            return string.Format("{0}{1}", name, this.Extension).Replace(' ', '_');
        }

        public override int GetHashCode()
        {
            return this.Extension.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(FileType))
            {
                return this.ToString().ToUpper() == obj.ToString().ToUpper();
            }

            return false;
        }

        public override string ToString()
        {
            return this.Extension;
        }
    }
}