using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryableFilters.Tests.DataContext;

namespace QueryableFilters.Tests
{
    [TestClass]
    public class ContainsTests
    {
        private NorthwindEntities Context;

        [TestMethod]
        public void ContainsNotNull()
        {
            var filterValue = "test";

            var correct = Context.Set<Supplier>().Where(x => x.ContactName.Contains(filterValue)).ToString();
            var tested = Context.Set<Supplier>().FilterContains(x => x.ContactName, filterValue).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void ContainsNullNotSkipEmptyArgs()
        {
            var correct = Context.Set<Supplier>().Where(x => x.ContactName == null).ToString();
            var tested = Context.Set<Supplier>().FilterContains(x => x.ContactName, null, false).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestInitialize]
        public void Init()
        {
            Context = new NorthwindEntities();
        }
    }
}
