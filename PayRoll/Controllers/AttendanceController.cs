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
         // GET: Attendance
         public ActionResult Index()
         {
             return View();
         }
 
         // GET: Attendance/Details/5
         public ActionResult Details(int id)
         {
             return View();
         }
 
         // GET: Attendance/Create
         public ActionResult Create()
         {
             return View();
         }
 
         // POST: Attendance/Create
         [HttpPost]
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
         public ActionResult Edit(int id)
         {
             return View();
         }
 
         // POST: Attendance/Edit/5
         [HttpPost]
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
         public ActionResult Delete(int id)
         {
             return View();
         }
 
         // POST: Attendance/Delete/5
         [HttpPost]
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
         public ActionResult ManageAttendance(string message)
         {
             ViewBag.StatusMessage = message;
             return View();
         }
 
         /*
          * Punch In:
          * User cannot punch in more than 1 hour before shift (to avoid time cheating)
          * If user clicks punch IN at the end of their shift, it should show error and prompt user to unch OUT
          */
         [HttpPost]
         public ActionResult PunchIn(Attendance model)
         {
            string error = "";
            Boolean admin = false;
            DateTime curTime = DateTime.Now;
            Boolean success = true;
            PayrollDbContext db = new PayrollDbContext();
            
            //db.SaveChanges();
            Employee user = db.Employees.Find(Session["EmployeeId"]);
            DateTime startTime = db.Schedules.Find(user.ShiftId).StartTime;

            // Check if user signs in too early
            double timeGap = (startTime.Hour - curTime.Hour);
            if (timeGap > 0.25)
            {
                success = false;
                error = "Your shift has not started yet. Please try again later.";
            }
            if (success)
            {
                db.Attendances.Add(new Attendance { Employee = user, SignInTime = curTime, SignOutTime = curTime });
                
                if ((Session["Position"].ToString() == "Manager") || (Session["Position"].ToString() == "Master"))
                {
                    admin = true;
                }
                if (!admin)
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Index", "Employees");
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
            PayrollDbContext db = new PayrollDbContext();

            //NEEDED MODIFICATION HERE
            Employee user = db.Employees.Find(Session["EmployeeId"]);

            var todayLog = db.Attendances.Where(m => m.Employee.EmployeeId == user.EmployeeId).FirstOrDefault();
            if (todayLog == null)
            {
                success = false;
                error = "You cannot punch out now. Please try again.";
            }

            //create new attendance log of the new work day (even if they forgot to punch out on the previous day)
            if (success)
            {
                todayLog.SignOutTime = curTime;
                db.Entry(todayLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageAttendance", new { Message = error });
            }
            else
                return RedirectToAction("Index", "Employees");
            //return RedirectToAction("Index", "PayrollManage");
		 }
 
     }
 }