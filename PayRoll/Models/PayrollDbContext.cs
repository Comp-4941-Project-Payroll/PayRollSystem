using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PayRoll.Models
{
	public class PayrollDbContext : DbContext
	{
		public PayrollDbContext(): base("name=PayrollDbContextConnectionString") 
        {
			Database.SetInitializer<PayrollDbContext>(new PayrollDbInitializer());
		}
		public DbSet<Employee> Employees { get; set; }
		public DbSet<TimeOffRequest> TimeOffRequests { get; set; }
		public DbSet<Position> Positions { get; set; }
		public DbSet<Branch> Branches { get; set; }
		public DbSet<Schedule> Schedules { get; set; }
		public DbSet<Attendance> Attendances { get; set; }
		public DbSet<Payroll> Payrolls { get; set; }
		//protected override void OnModelCreating(DbModelBuilder modelBuilder)
		//{
		//	modelBuilder.Entity<Employee>()
		//		   .HasOptional(e => e.TimeOffRequests)
		//		   .WithMany()
		//		   .WillCascadeOnDelete(true);
		//	base.OnModelCreating(modelBuilder);
		//}
	}
}