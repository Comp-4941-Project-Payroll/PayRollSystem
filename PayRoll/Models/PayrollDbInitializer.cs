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

            //TypeOfTimeOff toto = context.TypesOfTimeOff.Where(s => s.Type == "Personal Reasons").FirstOrDefault();

            TimeOffRequest tor = new TimeOffRequest()
            {
                StartDate = DateTime.Parse("2018-3-1"),
                EndDate = DateTime.Parse("2018-3-7"),
                Reason = "Vacation"
            };

            //toto.TimeOffRequests.Add(tor);

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
                HourlyRate = 35.00m,
                AwardedVacation = 15
			};


            e.Attendances.Add(new Attendance()
            {
                AttendanceId = "1",
                SignInTime = DateTime.Parse("2018-1-1 8:00:00 AM"),
                SignOutTime = DateTime.Parse("2018-1-1 8:00:00 PM"),
                
            });

            e.Attendances.Add(new Attendance()
            {
                AttendanceId = "2",
                SignInTime = DateTime.Parse("2018-2-1 8:00:00 AM"),
                SignOutTime = DateTime.Parse("2018-2-1 6:00:00 PM"),

            });


            e.TimeOffRequests.Add(tor);
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
