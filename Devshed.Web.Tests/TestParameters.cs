using System;
namespace Devshed.Web.Tests
{
    public sealed class TestParameters
    {
        public int UserId { get; set; }

        public string SearchText { get; set; }
    }

    public sealed class TestArrayParameters
    {
        public int[] Users { get; set; }

        public string SearchText { get; set; }
    }
}
