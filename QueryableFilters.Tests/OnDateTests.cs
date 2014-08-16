using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryableFilters.Tests.DataContext;

namespace QueryableFilters.Tests
{
    [TestClass]
    public class OnDateTests
    {
        private NorthwindEntities Context;

        [TestMethod]
        public void OnDateFilterNull()
        {
            var correct = Context.Set<Employee>().ToString();
            var tested = Context.Set<Employee>().FilterOnDate(x => x.BirthDate, null).ToString();
            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void OnDateOnNullableField()
        {
            var filterOnDate = DateTime.Now;

            var filterValueFrom = filterOnDate.Date;
            var filterValueTo = filterOnDate.Date.AddDays(1);

            var correct = Context.Set<Employee>().Where(x => x.BirthDate >= filterValueFrom)
                                 .Where(x => x.BirthDate < filterValueTo).ToString();
            var tested = Context.Set<Employee>().FilterOnDate(x => x.BirthDate, filterOnDate).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void OnDateOnNotNullableField()
        {
            var filterOnDate = DateTime.Now;

            var filterValueFrom = filterOnDate.Date;
            var filterValueTo = filterOnDate.Date.AddDays(1);

            var correct = Context.Set<Employee>().Where(x => x.CreatedDate >= filterValueFrom)
                                 .Where(x => x.CreatedDate < filterValueTo).ToString();
            var tested = Context.Set<Employee>().FilterOnDate(x => x.CreatedDate, filterOnDate).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestInitialize]
        public void Init()
        {
            Context = new NorthwindEntities();
        }
    }
}
