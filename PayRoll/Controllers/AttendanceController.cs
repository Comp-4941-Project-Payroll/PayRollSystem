using PayRoll.Models;
 using System;
 using System.Collections.Generic;
 using System.Data.Entity;
 using System.Linq;
 using System.Web;
 using System.Web.Mvc;
 
 namespace PayRoll.Controllers
 {
     public class AttendanceController : Controller
     {
        static PayrollDbContext db = new PayrollDbContext();
        // GET: Attendance
        [VerifyLogin]
        public ActionResult Index()
        {
            string empID = Session["EmployeeId"].ToString();
            Employee curEmployee = db.Employees.Where(e => e.EmployeeId == empID).FirstOrDefault();
            Attendance[] allAttendance = curEmployee.Attendances.ToArray();
            return View(allAttendance);
        }

        // GET: Attendance/Details/5
        [VerifyLogin]
		public ActionResult Details(int id)
         {
             return View();
         }

		// GET: Attendance/Create
		[VerifyLogin]
		public ActionResult Create()
         {
             return View();
         }
 
         // POST: Attendance/Create
         [HttpPost]
		[VerifyLogin]
		public ActionResult Create(FormCollection collection)
         {
             try
             {
                 // TODO: Add insert logic here
 
                 return RedirectToAction("Index");
             }
             catch
             {
                 return View();
             }
         }

		// GET: Attendance/Edit/5
		[VerifyLogin]
		public ActionResult Edit(int id)
         {
             return View();
         }
 
         // POST: Attendance/Edit/5
         [HttpPost]
		[VerifyLogin]
		public ActionResult Edit(int id, FormCollection collection)
         {
             try
             {
                 // TODO: Add update logic here
 
                 return RedirectToAction("Index");
             }
             catch
             {
                 return View();
             }
         }

		// GET: Attendance/Delete/5
		[VerifyLogin]
		public ActionResult Delete(int id)
         {
             return View();
         }
 
         // POST: Attendance/Delete/5
         [HttpPost]
		[VerifyLogin]
		public ActionResult Delete(int id, FormCollection collection)
         {
             try
             {
                 // TODO: Add delete logic here
 
                 return RedirectToAction("Index");
             }
             catch
             {
                 return View();
             }
         }

		// GET: /Manage/ManageAttendances
		[VerifyLogin]
		public ActionResult ManageAttendance(string message)
         {
             ViewBag.StatusMessage = message;
             return View();
         }

        [HttpPost]
        public ActionResult PunchIn(Attendance model)
        {
            string error = "";
            DateTime curTime = DateTime.Now;
            Boolean success = true;


            Employee user = null;
            Schedule[] scheds = db.Schedules.Include(e => e.Employees).ToArray();
            DateTime startTime = curTime;
            foreach (Schedule sched in scheds)
            {
                Employee emp = sched.Employees.Where(e => e.EmployeeId == Session["EmployeeId"].ToString()).FirstOrDefault();
                if (emp != null)
                {
                    user = emp;
                    startTime = sched.StartTime;
                    break;
                }
            }
            Attendance todayLog = user.Attendances.Where(m => m.SignInTime == m.SignOutTime).FirstOrDefault();
            if (todayLog != null && todayLog.SignOutTime.Date == curTime.Date)
            {
                success = false;
                error = "You already punched IN today. Please punch OUT.";
            }
            else if (startTime != curTime)
            {
                // Check if user signs in too early
                double timeGap = (startTime.Hour - curTime.Hour);
                if (timeGap > 0.25)
                {
                    success = false;
                    error = "Your shift has not started yet. Please try again later.";
                }
            }
            else
            {
                success = false;
                error = "You have not been assigned to any shift yet. Please contact admin for more information.";
            }
            if (success)
            {
                user.Attendances.Add(new Attendance { SignInTime = curTime, SignOutTime = curTime });
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            else
                return RedirectToAction("ManageAttendance", new { Message = error });
            //return RedirectToAction("Index", "PayrollManage");

        }

        [HttpPost]
        public ActionResult PunchOut(Attendance model)
        {
            DateTime curTime = DateTime.Now;
            string error = "";
            Boolean success = true;
            //PayrollDbContext db = new PayrollDbContext();

            //NEEDED MODIFICATION HERE
            string userID = Session["EmployeeId"].ToString();
            Employee user = db.Employees.Where(e => e.EmployeeId.Equals(userID)).FirstOrDefault();
            //Attendance userTodayLog = null;
            //Schedule[] scheds = db.Schedules.Include(e => e.Employees).Where(e=>e.StartTime == e.EndTime).ToArray();
            Attendance[] logs = user.Attendances.ToArray();
            Attendance todayLog = null;
            foreach (Attendance log in logs)
            {

                if (DateTime.Compare(log.SignInTime, log.SignOutTime) == 0 && log.SignInTime.Date == curTime.Date)
                {
                    todayLog = log;
                    break;
                }
            }
            if (todayLog == null)
            {
                success = false;
                error = "You cannot punch OUT. Please try again.";
            }

            //create new attendance log of the new work day (even if they forgot to punch out on the previous day)
            if (success)
            {
                todayLog.SignOutTime = curTime;
                db.Entry(todayLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("ManageAttendance", new { Message = error });

            //return RedirectToAction("Index", "PayrollManage");
        }
    }
}