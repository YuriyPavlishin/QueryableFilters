using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryableFilters.Tests.DataContext;
using System.Linq;

namespace QueryableFilters.Tests
{
    [TestClass]
    public class DateRangeTests
    {
        private NorthwindEntities Context;

        [TestMethod]
        public void DateRangeForOneFieldNotNull()
        {
            var filterValueFrom = DateTime.Now;
            var filterValueTo = DateTime.Now;

            var correct = Context.Set<Employee>()
                                 .Where(x => x.BirthDate >= filterValueFrom)
                                 .Where(x => x.BirthDate < filterValueTo).ToString();

            var tested = Context.Set<Employee>().FilterDateRange(x => x.BirthDate, filterValueFrom, filterValueTo).ToString();
            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void DateRangeForOneFieldFromFilterNull()
        {
            var filterValueTo = DateTime.Now;

            var correct = Context.Set<Employee>()
                .Where(x => x.BirthDate < filterValueTo).ToString();
            var tested = Context.Set<Employee>().FilterDateRange(x => x.BirthDate, null, filterValueTo).ToString();
            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void DateRangeForOneFieldAllFiltersNull()
        {
            var correct = Context.Set<Employee>().ToString();
            var tested = Context.Set<Employee>().FilterDateRange(x => x.BirthDate, null, null).ToString();
            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void DateRangeForTwoFieldNotNull()
        {
            var filterValueFrom = DateTime.Now;
            var filterValueTo = DateTime.Now;
            /*In this scenarion
              x.BirthDate - FromDate
              x.HireDate - ToDate             
             */
            
            var correct = Context.Set<Employee>()
                                 .Where(x => x.HireDate >= filterValueFrom)
                                 .Where(x => x.BirthDate < filterValueTo).ToString();

            var tested = Context.Set<Employee>().FilterDateRange(x => x.BirthDate, x=>x.HireDate, filterValueFrom, filterValueTo).ToString();
            Assert.AreEqual(correct, tested);
        }

        [TestInitialize]
        public void Init()
        {
            Context = new NorthwindEntities();
        }
    }
}
