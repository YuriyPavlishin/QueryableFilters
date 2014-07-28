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

            var correct = Context.Set<Employee>().Where(x => x.Title.Contains(filterValue)).ToString();
            var tested = Context.Set<Employee>().FilterContains(x => x.Title, filterValue).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void ContainsCompositeNotNull()
        {
            var filterValue = "test";

            var correct = Context.Set<Employee>().Where(x => (x.FirstName + " " + x.LastName).Contains(filterValue)).ToString();
            var tested = Context.Set<Employee>().FilterContains(x => x.FirstName + " " + x.LastName, filterValue).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void ContainsNullNotSkipEmptyArgs()
        {
            var correct = Context.Set<Employee>().Where(x => x.Title == null).ToString();
            var tested = Context.Set<Employee>().FilterContains(x => x.Title, null, false).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestInitialize]
        public void Init()
        {
            Context = new NorthwindEntities();
        }
    }
}
