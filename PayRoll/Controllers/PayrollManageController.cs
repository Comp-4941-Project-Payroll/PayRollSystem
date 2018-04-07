using PayRoll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PayRoll.Controllers
{
    public class PayrollManageController : Controller
    {
        private PayrollDbContext db = new PayrollDbContext();

        // GET: PayrollManage
        public ActionResult Index()
        {
            

            return View(db.Payrolls.ToList());
        }

        // GET: PayrollManage/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("index");
            }

            string[] strings = id.Split(' ');
            DateTime month = new DateTime(DateTime.Today.Year, Int32.Parse(strings[0]), 1);
            DateTime firstDayOfPeriod;
            DateTime lastDayOfPeriod;

            if (Int32.Parse(strings[1]) == 1)
            {
                firstDayOfPeriod = month;
                lastDayOfPeriod = month.AddDays(14);
            }
            else
            {
                firstDayOfPeriod = month.AddDays(15);
                lastDayOfPeriod = month.AddMonths(1).AddDays(-1);
            }

            string dateRange = firstDayOfPeriod.ToShortDateString() + " - " + lastDayOfPeriod.ToShortDateString();
            ViewBag.dateRange = dateRange;

            List<Attendance> attendances = null;
            List<Attendance> attendancesYTD = null;
            List<TimeOffRequest> timeOff = null;
            List<Employee> emps = db.Employees.Include(e => e.Attendances).ToList();
            List<Employee> emps2 = db.Employees.Include(e => e.TimeOffRequests).ToList();

            double hours = 0;
            double hoursYTD = 0;
            double overtimeHrs = 0;
            double overtimeHrsYTD = 0;
            decimal earnings = 0;
            decimal earningsYTD;
            decimal rate = 0;
            double vacationDays = 0;
            int awardedVacation = 0;
            int remainingVacation = 0;
            decimal eiAmount = 0;
            decimal eiAmountYTD = 0;
            decimal cppAmount = 0;
            decimal cppAmountYTD = 0;
            decimal taxAmount = 0;
            decimal taxAmountYTD = 0;
            decimal netPay = 0;
            decimal netPayYTD = 0;
            foreach (Employee emp in emps)
            {
                if (emp.EmployeeId == "a00828730")
                {
                    attendances = emp.Attendances
                        .Where(e => e.SignInTime.Year == firstDayOfPeriod.Year && e.SignInTime.Month == firstDayOfPeriod.Month
                            && e.SignInTime.Day >= firstDayOfPeriod.Day && e.SignInTime.Day <= lastDayOfPeriod.Day).ToList();
                    attendancesYTD = emp.Attendances
                        .Where(e => e.SignInTime.Year == firstDayOfPeriod.Year && e.SignInTime.DayOfYear <= lastDayOfPeriod.DayOfYear).ToList();
                    rate = emp.HourlyRate;
                    awardedVacation = emp.AwardedVacation;
                    break;
                }
            }

            if (attendances != null)
            {
                foreach (Attendance a in attendances)
                {
                    if ((a.SignOutTime - a.SignInTime).TotalHours <= 8)
                    {
                        hours += (a.SignOutTime - a.SignInTime).TotalHours;
                    }
                    else
                    {
                        hours += 8;
                        overtimeHrs += (a.SignOutTime - a.SignInTime).TotalHours - 8;
                    }

                }
            }

            earnings = (decimal)hours * rate + (decimal)overtimeHrs * rate * (decimal)1.5;

            if (attendancesYTD != null)
            {
                foreach (Attendance a in attendancesYTD)
                {
                    if ((a.SignOutTime - a.SignInTime).TotalHours <= 8)
                    {
                        hoursYTD += (a.SignOutTime - a.SignInTime).TotalHours;
                    }
                    else
                    {
                        hoursYTD += 8;
                        overtimeHrsYTD += (a.SignOutTime - a.SignInTime).TotalHours - 8;
                    }

                }
            }

            earningsYTD = (decimal)hoursYTD * rate + (decimal)overtimeHrsYTD * rate * (decimal)1.5;

            foreach (Employee emp in emps2)
            {
                if (emp.EmployeeId == "a00828730")
                {
                    timeOff = emp.TimeOffRequests.Where(t => t.StartDate.Year == firstDayOfPeriod.Year
                        && t.EndDate <= lastDayOfPeriod).ToList();
                    break;
                }
            }


            foreach (TimeOffRequest timeaway in timeOff)
            {
                vacationDays += (timeaway.EndDate - timeaway.StartDate).TotalDays + 1;
            }

            remainingVacation = awardedVacation - (int)vacationDays;
            eiAmount = earnings * (Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["EI"])/100);
            cppAmount = earnings * (Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["CPP"]) / 100);
            taxAmount = earnings * (Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["Tax"]) / 100);

            eiAmountYTD = earningsYTD * (Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["EI"]) / 100);
            cppAmountYTD = earningsYTD * (Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["CPP"]) / 100);
            taxAmountYTD = earningsYTD * (Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["Tax"]) / 100);

            netPay = earnings - eiAmount - cppAmount - taxAmount;
            netPayYTD = earningsYTD - eiAmountYTD - cppAmountYTD - taxAmountYTD;
            //Console.WriteLine(hours);

            ViewBag.Hours = hours;
            ViewBag.HoursYTD = hoursYTD;
            ViewBag.Earnings = earnings;
            ViewBag.EarningsYTD = earningsYTD;
            ViewBag.VacationHrs = vacationDays;
            ViewBag.AwardedVacation = awardedVacation;
            ViewBag.RemainingVacation = remainingVacation;
            ViewBag.OvertimeHrs = overtimeHrs;
            ViewBag.OvertimeHrsYTD = overtimeHrsYTD;
            ViewBag.EI = eiAmount;
            ViewBag.CPP = cppAmount;
            ViewBag.Tax = taxAmount;
            ViewBag.NetPay = netPay;
            ViewBag.EIYTD = eiAmountYTD;
            ViewBag.CPPYTD = cppAmountYTD;
            ViewBag.TaxYTD = taxAmountYTD;
            ViewBag.NetPayYTD = netPayYTD;

            return View();
        }

        // GET: PayrollManage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PayrollManage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PayrollId,year,RegularHours,OverTimeHours,HourlyRate,Earnings,BeginningVacation,VacationHrsTaken,CPP,EI,IncomeTaxes")] Payroll payroll)
        {
            if (ModelState.IsValid)
            {
                db.Payrolls.Add(payroll);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payroll);
        }

        // GET: PayrollManage/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payroll payroll = db.Payrolls.Find(id);
            if (payroll == null)
            {
                return HttpNotFound();
            }
            return View(payroll);
        }

        // POST: PayrollManage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PayrollId,year,RegularHours,OverTimeHours,HourlyRate,Earnings,BeginningVacation,VacationHrsTaken,CPP,EI,IncomeTaxes")] Payroll payroll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payroll).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payroll);
        }

        // GET: PayrollManage/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payroll payroll = db.Payrolls.Find(id);
            if (payroll == null)
            {
                return HttpNotFound();
            }
            return View(payroll);
        }

        // POST: PayrollManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Payroll payroll = db.Payrolls.Find(id);
            db.Payrolls.Remove(payroll);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
