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
				PositionId = "Admin"
			});

            context.Positions.Add(new Position()
			{
				PositionId = "Human Resources"
			});

			context.SaveChanges();

			context.Employees.Add(new Employee()
			{
				EmployeeId = "a00828729",
                Password = "1234567890",
				FName = "Davin",
				LName = "Deol",
				Address = "4652 Redex Blvd",
				Email = "vpnprez@hotmail.com",
				FullOrPartTime = "Part-Time",
				Seniority = 4,
				DepartmentType = "Executive",
                HourlyRate = 35.00m
			});
			context.SaveChanges();
			context.Positions.Find("Admin").Employees.Add(context.Employees.Find("a00828729"));
			context.SaveChanges();
			context.TypesOfTimeOff.Add(new TypeOfTimeOff()
            {
                Type = "Vacation"
            });
            context.TypesOfTimeOff.Add(new TypeOfTimeOff()
            {
                Type = "Personal Reasons"
            });
            context.TypesOfTimeOff.Add(new TypeOfTimeOff()
            {
                Type = "Appointment"
            });

    Employee e = new Employee()
            {
                EmployeeId = "a00828730",
                Password = "1020304050",
                FName = "Andra",
                LName = "Avram",
                Address = "Pinetree Way",
                Email = "vpnprez1@hotmail.com",
                FullOrPartTime = "Full-Time",
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
