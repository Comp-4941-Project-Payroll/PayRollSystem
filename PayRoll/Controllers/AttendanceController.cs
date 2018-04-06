﻿using PayRoll.Models;
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
            DateTime curTime = DateTime.Now;
            Boolean success = true;
            PayrollDbContext db = new PayrollDbContext();
            db.Schedules.Add(new Schedule { ShiftId = "03", StartTime = new DateTime(2018, 04, 6, 16, 00, 00), EndTime = new DateTime(2018, 04, 7, 00, 00, 00) });
            //NEEDED MODIFICATION HERE
            Employee user = db.Employees.Find("a00828729");
            DateTime startTime = db.Schedules.Find(user.ShiftIdd).StartTime;

            // Check if user signs in too early
            if ((curTime - startTime).TotalHours < -1)
            {
                success = false;
                error = "Your shift has not started yet. Please try again later.";
            }
            // Check if user has already signed in earlier in the day, just in case they mispunch
            else if (db.Attendances.Where(m => m.EmployeeId == user && m.SignInTime.Date == curTime.Date && m.SignOutTime == null) != null)
            {
                success = false;
                error = "You have already punched in today. Please try again.";
            }
            //create new attendance log of the new work day (even if they forgot to punch out on the previous day)
            if (success)
            {
                db.Attendances.Add(new Attendance { EmployeeId = user, SignInTime = curTime });
            }
            return success ? RedirectToAction("Index") : RedirectToAction("ManageAttendance", new { Message = error });

        }

        [HttpPost]
        public ActionResult PunchOut(Attendance model)
        {
            DateTime curTime = DateTime.Now;
            string error = "";
            Boolean success = true;
            PayrollDbContext db = new PayrollDbContext();

            //NEEDED MODIFICATION HERE
            Employee user = db.Employees.Find("a00828729");
            //db.Schedules.Add(new Schedule { ShiftId = "03", StartTime = new DateTime(2018, 04, 6, 16, 00, 00), EndTime = new DateTime(2018, 04, 7, 00, 00, 00) });
            Attendance todayLog = db.Attendances.Where(m => m.EmployeeId == user && m.SignInTime.Date == curTime.Date).FirstOrDefault();
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
                return RedirectToAction("Index");
        }

    }
}
