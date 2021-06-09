using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
using System.ComponentModel;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class EmployeeController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<EmployeeItem> Employee_EmployeeCustomers()
        {
            using (var context = new ChinookSystemContext())
            {
				//In your application we are using Linq to Entity
				//I need to alter the from to indicate where the Entity
				//  DbSets are located
				IEnumerable<EmployeeItem> results = from x in context.Employees
							   where x.Title.Contains("Sales Support")
							   orderby x.LastName, x.FirstName
							   select new EmployeeItem
							   {
								   FullName = x.LastName + ", " + x.FirstName,
								   Title = x.Title,
								   NumberOfCustomers = x.Customers.Count(),
								   CustomerList = (from y in x.Customers
												   select new CustomerItem
												   {
													   FullName = y.LastName + ", " + y.FirstName,
													   Phone = y.Phone,
													   City = y.City,
													   State = y.State
												   }).ToList()
							   };
				return results.ToList();
			}
        }
    }
}
