namespace Devshed.IO
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using Devshed.Shared;

    public interface IFileContainer
    {
        Stream InnerStream { get; }

        FileType FileType { get; }

        string FormatName(string name);
    }

    [Serializable]
    public sealed class FileContainer : IFileContainer, IDisposable
    {
        private readonly Stream stream;

        //// Required for serialization.
        public FileContainer()
        {
        }

        /// <summary>
        /// Accepts a filetype and stream and returns the stream data from the beginning.
        /// </summary>
        /// <param name="type"> Filetype </param>
        /// <param name="stream"> Stream to write from beginning. </param>
        public FileContainer(FileType type, Stream stream)
        {
            this.stream = stream;
            this.FileType = type;

            this.stream.Position = 0;
        }

        public Stream InnerStream
        {
            get
            {
                return this.stream;
            }
        }

        public FileType FileType { get; set; }

        public byte[] GetBytes()
        {
            return this.stream.GetBytes();
        }

        [XmlIgnore]
        public string ContentType
        {
            get
            {
                return this.FileType.ContentType;
            }
        }

        public string FormatName(string name)
        {
            if (this.FileType == null)
            {
                throw new InvalidOperationException("No file type for formatting is specified.");
            }

            return this.FileType.FormatName(name);
        }

        public void Dispose()
        {
            this.InnerStream.Dispose();
        }
    }
}
