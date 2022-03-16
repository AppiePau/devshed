namespace Devshed.IO
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using Devshed.Shared;

    /// <summary>
    /// An interface to represent a file container.
    /// </summary>
    public interface IFileContainer
    {
        /// <summary>
        /// The stream that holds the file contents.
        /// </summary>
        Stream InnerStream { get; }

        /// <summary>
        /// The file type 'defintion', for extension and content type.
        /// </summary>
        FileType FileType { get; }

        /// <summary>
        /// A method to create a file based on the extension in the FileType.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string FormatName(string name);
    }

    /// <summary>
    /// The physical implementation of a filecontainer.
    /// </summary>
    [Serializable]
    public sealed class FileContainer : IFileContainer, IDisposable
    {
        private readonly Stream stream;

        /// <summary>
        /// Initialize a container, required for serialization.
        /// </summary>
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

        /// <summary>
        /// The content stream of the file being transferred.
        /// </summary>
        public Stream InnerStream
        {
            get
            {
                return this.stream;
            }
        }

        /// <summary>
        /// The file type that is being transferred.
        /// </summary>
        public FileType FileType { get; set; }

        /// <summary>
        /// Returns the stream as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return this.stream.GetBytes();
        }

        /// <summary>
        /// A shortcut to return the content type.
        /// </summary>
        [XmlIgnore]
        public string ContentType
        {
            get
            {
                return this.FileType.ContentType;
            }
        }

        /// <summary>
        /// A method to create a file based on the extension in the FileType.
        /// </summary>
        /// <param name="name"> The name of the file (without extension) to format. </param>
        /// <returns></returns>
        public string FormatName(string name)
        {
            if (this.FileType == null)
            {
                throw new InvalidOperationException("No file type for formatting is specified.");
            }

            return this.FileType.FormatName(name);
        }

        /// <summary>
        /// Disposes the container and closes the stream.
        /// </summary>
        public void Dispose()
        {
            this.InnerStream.Dispose();
        }
    }
}
