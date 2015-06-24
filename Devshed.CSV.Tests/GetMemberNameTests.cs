namespace Devshed.Csv.Tests
{
    using System;
    using System.Linq.Expressions;
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetMemberNameTests
    {
        [TestMethod]
        public void GetMemberName_ValidExpression_ReturnsName()
        {
            Expression<Func<Model, decimal>> expression = (x) => x.Price;
            Assert.AreEqual("Price", expression.GetMemberName());
        }

        private sealed class Model
        {
            public decimal Price { get; set; }
        }
    }
}
