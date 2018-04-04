using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class PayrollDbInitializer : DropCreateDatabaseAlways<PayrollDbContext>
	{
		protected override void Seed(PayrollDbContext context)
		{
			Position adminPosition = new Position()
			{
				PositionId = "AD31N",
				PositionName = "Admin",
				BaseSalary = (decimal) 35.50
			};
			context.Positions.Add(adminPosition);
			Employee adminEmployee = new Employee()
			{
				EmployeeId = "0000-0000-0000-0000-0000",
                Password = "1234567890",
				FName = "Davin",
				LName = "Deol",
				Address = "4652 Redex Blvd",
				Phone = "778-535-8435",
				FullOrPartTime = "FullTime",
				Seniority = 4,
				DepartmentType = "Executive"
			};
			context.Employees.Add(adminEmployee);
            context.SaveChanges();
            context.Positions.Find("AD31N").Employees.Add(adminEmployee);
            context.SaveChanges();
		}
	}
}