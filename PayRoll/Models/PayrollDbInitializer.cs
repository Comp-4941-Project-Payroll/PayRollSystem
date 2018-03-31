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
				positionName = "Admin",
				baseSalary = (decimal) 35.50
			};
			Employee adminEmployee = new Employee()
			{
				EmployeeID = new Guid().ToString(),
				password = "123",
				fName = "Admin",
				lName = "Istrator",
				address = "4652 Redex Blvd",
				phone = "778-535-8435",
				fullOrPartTime = "FullTime",
				PositionId = adminPosition,
				seniority = 4,
				departmentType = "Executive"
			};
		}
	}
}