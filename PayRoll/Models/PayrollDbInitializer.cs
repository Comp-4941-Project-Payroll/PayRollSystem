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
				EmployeeId = new Guid().ToString(),
				Password = "1234567890",
				FName = "Davin",
				LName = "Deol",
				Address = "4652 Redex Blvd",
				Phone = "778-535-8435",
				FullOrPartTime = "FullTime",
				PositionId = adminPosition,
				Seniority = 4,
				DepartmentType = "Executive"
			};
			context.Employees.Add(adminEmployee);
			context.SaveChanges();
		}
	}
}