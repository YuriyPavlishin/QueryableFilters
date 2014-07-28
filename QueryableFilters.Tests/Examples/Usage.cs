using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryableFilters.Tests.DataContext;

namespace QueryableFilters.Tests.Examples
{
    public class Usage
    {
        public List<Employee> GetByWithoutFilters(FilterArgs args)
        {
            var Context = new NorthwindEntities();

            var qEmployee = Context.Set<Employee>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(args.FullName))
            {
                var fullNameTrimmed = args.FullName.Trim();
                qEmployee = qEmployee.Where(x => (x.FirstName + " " + x.LastName).Contains(fullNameTrimmed));
            }

            if (args.EmployeeTypes1 != null)
            {
                qEmployee = qEmployee.Where(x => x.EmployeeType1 == args.EmployeeTypes1);
            }

            if (args.ReportsTo != null)
            {
                qEmployee = qEmployee.Where(x => x.ReportsTo == args.ReportsTo);
            }

            if (args.CreatedFrom != null)
            {
                var createdFrom = args.CreatedFrom.Value.Date;
                qEmployee = qEmployee.Where(x => x.CreatedDate >= createdFrom);
            }

            if (args.CreatedTo != null)
            {
                var createdTo = args.CreatedTo.Value.Date.AddDays(1);
                qEmployee = qEmployee.Where(x => x.CreatedDate < createdTo);
            }

            return qEmployee.ToList();
        }

        public List<Employee> GetByWithFilters(FilterArgs args)
        {
            var Context = new NorthwindEntities();

            return Context.Set<Employee>()
                    .FilterContains(x => x.FirstName + " " + x.LastName, args.FullName)
                    .FilterEquals(x => x.EmployeeType1, args.EmployeeTypes1)
                    .FilterEquals(x => x.ReportsTo, args.ReportsTo)
                    .FilterDateRange(x => x.CreatedDate, args.CreatedFrom, args.CreatedTo)
                    .ToList();
        }

        public class FilterArgs
        {
            public string FullName { get; set; }
            public Enums.EmployeeTypes? EmployeeTypes1 { get; set; }
            public int? ReportsTo { get; set; }
            public DateTime? CreatedFrom { get; set; }
            public DateTime? CreatedTo { get; set; }
        }
    }
}
