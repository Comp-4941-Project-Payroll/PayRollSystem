using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PayRoll.Models;
using PayRoll.Models.Repository;

namespace PayRoll.Controllers
{
    public class TimeOffRequestsController : Controller
    {
        private PayrollDbContext db = new PayrollDbContext();

        // GET: TimeOffRequests
        public ActionResult Index()
        {
			ViewData["typesOfTimeOff"] = db.TypesOfTimeOff.ToArray();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "StartDate,EndDate,Reason")] TimeOffRequest timeOffRequest)
        {
            if ((timeOffRequest.StartDate < DateTime.Now)
                || (timeOffRequest.EndDate < DateTime.Now)
                || (timeOffRequest.StartDate > timeOffRequest.EndDate))
			{
				ViewData["typesOfTimeOff"] = db.TypesOfTimeOff.ToArray();
				return View(timeOffRequest);
            }
            //TimeOffRequestId,WhenSent
            timeOffRequest.WhenSent = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    db.TimeOffRequests.Add(timeOffRequest);
                    db.SaveChanges();
                    db.Employees.Find("a00828729").TimeOffRequests.Add(timeOffRequest);
                    db.SaveChanges();
                    db.TypesOfTimeOff.Find(Request.Form.Get("Type")).TimeOffRequests.Add(timeOffRequest);
                    db.SaveChanges();
                } catch (Exception e) {
                    return RedirectToAction("Failure");
                }
                return RedirectToAction("Success");
            }

			ViewData["typesOfTimeOff"] = db.TypesOfTimeOff.ToArray();
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
