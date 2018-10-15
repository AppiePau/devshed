namespace Devshed.Csv
{
    using Devshed.Csv.Reading;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public sealed class Header : IComparer<string>
    {
        public Header(string name)
        {
            this.Name = name;
            this.Key = name.ToUpper();
        }

        public string Name { get; }

        public string Key { get; }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Key.Equals((obj as Header)?.Key);
        }

        public int Compare(string x, string y)
        {
            return x.CompareTo(y);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }


    public sealed class HeaderCollection : ReadOnlyCollection<Header>
    {
        public HeaderCollection(params string[] headers) : this(headers.Select(e => new Header(e)).ToList())
        {
        }

        public HeaderCollection(IList<Header> headers) : base(headers)
        {
            if (headers.GroupBy(x => x.Key).Any(g => g.Count() > 1))
            {
                var duplicates = headers.GroupBy(x => x.Key).Where(g => g.Count() > 1).SelectMany(e => e.Select(x => x.Name)).ToArray();
                throw new DuplicateHeaderException($"A duplicate header was found: " + string.Join(", ", duplicates));
            }

            this.Length = headers.Count();
        }

        public int Length { get; }

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
