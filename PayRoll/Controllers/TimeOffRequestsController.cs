using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PayRoll.Models;

namespace PayRoll.Controllers
{
    public class TimeOffRequestsController : Controller
    {
        private PayrollDbContext db = new PayrollDbContext();

        // GET: TimeOffRequests
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Type,StartDate,EndDate,Reason")] TimeOffRequest timeOffRequest)
        {
            //TimeOffRequestId,WhenSent
            timeOffRequest.WhenSent = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.TimeOffRequests.Add(timeOffRequest);
                db.SaveChanges();
                db.Employees.Find("0000-0000-0000-0000-0000").TimeOffRequests.Add(timeOffRequest);
                db.SaveChanges();
                return RedirectToAction("Success");
            }

            return View(timeOffRequest);
        }

        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Failure()
        {
            return View();
        }
    }
}
