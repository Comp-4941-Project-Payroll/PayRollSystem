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
            context.Positions.Add(new Position()
			{
				PositionId = "AD31N",
				PositionName = "Admin",
				BaseSalary = (decimal) 35.50
			});
            Employee e = new Employee()
            {
                EmployeeId = "0000-0000-0000-0000-0000",
                Password = "1234567890",
                FName = "Davin",
                LName = "Deol",
                Address = "4652 Redex Blvd",
                Phone = "778-535-8435",
                FullOrPartTime = "FullTime",
                Seniority = 4,
                DepartmentType = "Executive",
                HourlyRate = 35.00m
			};


            Attendance a = new Attendance()
            {
                AttendanceId = "1",
                SignInTime = DateTime.Parse("2018-1-1 8:00:00 AM"),
                SignOutTime = DateTime.Parse("2018-1-1 9:00:00 AM"),
                
            };

            e.Attendances.Add(a);

            context.Employees.Add(e);
            context.Attendances.Add(a);

            context.SaveChanges();
            base.Seed(context);
		}
	}
}
/*
 * TimeOffRequest x = context.TimeOffRequests.Find(10);
 * Employee y = context.Employees.Find("0000-0000-0000-0000-0000");
 * y.TimeOffRequests.Add(x);
 * db.Employees.Find("0000-0000-0000-0000-0000").TimeOffRequests.Add(timeOffRequest);
 */
