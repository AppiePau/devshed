namespace Devshed.Csv
{
    using Devshed.Csv.Reading;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>Represents a CSV header with a name and a key value.</summary>
    public sealed class Header : IComparer<string>
    {
        /// <summary>Initializes a new instance of the <see cref="Header" /> class.</summary>
        /// <param name="name">The name of the header.</param>
        public Header(string name)
        {
            this.Name = name;
            this.Key = name.ToUpper();
        }

        /// <summary>Gets the name of the header.</summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>Gets the escaped key.</summary>
        /// <value>The key.</value>
        public string Key { get; }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        /// <summary>Determines whether the specified <see cref="System.Object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return this.Key.Equals((obj as Header)?.Key);
        }

        /// <summary>Compares the specified x.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public int Compare(string x, string y)
        {
            return x.CompareTo(y);
        }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }


    /// <summary>Contains headers of the CSV file to either read or write.</summary>
    public sealed class HeaderCollection : ReadOnlyCollection<Header>
    {
        /// <summary>Initializes a new instance of the <see cref="HeaderCollection" /> class.</summary>
        /// <param name="headers">The headers.</param>
        public HeaderCollection(params string[] headers) : this(headers.Select(e => new Header(e)).ToList())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="HeaderCollection" /> class.</summary>
        /// <param name="headers">The headers to start with.</param>
        /// <exception cref="DuplicateHeaderException">A duplicate header was found: " + string.Join(", ", duplicates)</exception>
        public HeaderCollection(IList<Header> headers) : base(headers)
        {
            if (headers.GroupBy(x => x.Key).Any(g => g.Count() > 1))
            {
                var duplicates = headers.GroupBy(x => x.Key).Where(g => g.Count() > 1).SelectMany(e => e.Select(x => x.Name)).ToArray();
                throw new DuplicateHeaderException($"A duplicate header was found: " + string.Join(", ", duplicates));
            }

            this.Length = headers.Count();
        }

        /// <summary>Gets the length the header collection.</summary>
        /// <value>The length.</value>
        public int Length { get; }

        /// <summary>Gets the name of the header.</summary>
        /// <param name="index">The index.</param>
        /// <returns>A Header object cntaining the header name and key value.<br /></returns>
        /// <exception cref="InvalidOperationException">Getting header name failed. The index is out of header bounds.</exception>
        public Header GetHeaderName(int index)
        {
            if (index > this.Count() - 1)
            {
                throw new InvalidOperationException("Getting header name failed. The index is out of header bounds.");
            }

            return this[index];
        }
    }
}
