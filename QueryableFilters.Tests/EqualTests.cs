using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryableFilters.Tests.DataContext;
using System.Linq;

namespace QueryableFilters.Tests
{
    [TestClass]
    public class EqualsQueryGenerateTests
    {
        private NorthwindEntities Context;

        [TestMethod]
        public void IntEqual()
        {
            var filterValue = 1;

            var correct = Context.Set<Category>().Where(x => x.CategoryID == filterValue).ToString();
            var tested = Context.Set<Category>().FilterEquals(x => x.CategoryID, filterValue).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void IntNullEqual()
        {
            int? filterValue = null;

            var correct = Context.Set<Category>().ToString();
            var tested = Context.Set<Category>().FilterEquals(x => x.CategoryID, filterValue).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void IntNullEqualNotSkipNullArgs()
        {
            var correct = Context.Set<Order>()
                .Where(x=>x.EmployeeID == null).ToString();
            var tested = Context.Set<Order>().FilterEquals(x => x.EmployeeID, null, false).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void IntEqualOnDbNullField()
        {
            var filterValue = 1;

            var correctExpression = Context.Set<Order>().Where(x => x.EmployeeID == filterValue).Select(x=>x.EmployeeID);
            var correct = correctExpression.ToString();
            var testedExpression = Context.Set<Order>().FilterEquals(x => x.EmployeeID, filterValue).Select(x=>x.EmployeeID);
            var tested = testedExpression.ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void StringEquals()
        {
            var filterValue = "test";

            var correct = Context.Set<Supplier>().Where(x=>x.ContactName == filterValue).ToString();
            var tested = Context.Set<Supplier>().FilterEquals(x => x.ContactName, filterValue).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void StringWhitespaceEquals()
        {
            var filterValue = "   ";

            var correct = Context.Set<Supplier>().ToString();
            var tested = Context.Set<Supplier>().FilterEquals(x => x.ContactName, filterValue).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void BoolEquals()
        {
            var filterValue = true;

            var correct = Context.Set<Product>().Where(x => x.Discontinued == filterValue).ToString();
            var tested = Context.Set<Product>().FilterEquals(x => x.Discontinued, filterValue).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void BoolNullEquals()
        {
            bool? filterValue = null;

            var correct = Context.Set<Product>().ToString();
            var tested = Context.Set<Product>().FilterEquals(x => x.Discontinued, filterValue).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void DateTimeEquals()
        {
            var filterValue = DateTime.Now;

            var correct = Context.Set<Employee>().Where(x => x.CreatedDate == filterValue).Select(x=>x.EmployeeID).ToString();
            var tested = Context.Set<Employee>().FilterEquals(x => x.CreatedDate, filterValue).Select(x=>x.EmployeeID).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void DateTimeEqualsOnDbNullField()
        {
            var filterValue = DateTime.Now;

            var correct = Context.Set<Employee>().Where(x => x.BirthDate == filterValue).Select(x => x.EmployeeID).ToString();
            var tested = Context.Set<Employee>().FilterEquals(x => x.BirthDate, filterValue).Select(x => x.EmployeeID).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void EnumEqual()
        {
            var filterValue = Enums.EmployeeTypes.Type1;
            /*
                var correct = Context.Set<Employee>().Where(x => x.EmployeeType1 == filterValue).Select(x => x.EmployeeID).ToString();
                standard method generate query with cast for enums field
              
                SELECT 
                [Extent1].[EmployeeID] AS [EmployeeID]
                FROM [dbo].[Employees] AS [Extent1]
                WHERE  CAST( [Extent1].[EmployeeType1] AS int) = @p__linq__0>
             */
            
            var correct = "SELECT \r\n    [Extent1].[EmployeeID] AS [EmployeeID]\r\n    FROM [dbo].[Employees] AS [Extent1]\r\n    WHERE [Extent1].[EmployeeType1] = @p__linq__0";
            var tested = Context.Set<Employee>().FilterEquals(x => x.EmployeeType1, filterValue).Select(x => x.EmployeeID).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestMethod]
        public void EnumEqualOnDbNullField()
        {
            var filterValue = Enums.EmployeeTypes.Type1;
            /*
                var correct = Context.Set<Employee>().Where(x => x.EmployeeType2 == filterValue).Select(x => x.EmployeeID).ToString();
                standard method generate query with cast for enums field
              
                SELECT 
                [Extent1].[EmployeeID] AS [EmployeeID]
                FROM [dbo].[Employees] AS [Extent1]
                WHERE  CAST( [Extent1].[EmployeeType1] AS int) = @p__linq__0>
             */

            var correct = "SELECT \r\n    [Extent1].[EmployeeID] AS [EmployeeID]\r\n    FROM [dbo].[Employees] AS [Extent1]\r\n    WHERE [Extent1].[EmployeeType2] = @p__linq__0";
            var tested = Context.Set<Employee>().FilterEquals(x => x.EmployeeType2, filterValue).Select(x => x.EmployeeID).ToString();

            Assert.AreEqual(correct, tested);
        }

        [TestInitialize]
        public void Init()
        {
            Context = new NorthwindEntities();
        }
    }}
