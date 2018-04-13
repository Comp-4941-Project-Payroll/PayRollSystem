using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PayRoll.Models;
using PayRoll.Models.Repository;

namespace PayRoll.Controllers
{
    public class TimeOffRequestsController : Controller
    {
        private PayrollDbContext db = new PayrollDbContext();
        private string sessionEmployee = System.Web.HttpContext.Current.Session["EmployeeId"] as String;

        // GET: TimeOffRequests
		[VerifyLogin]
        public ActionResult Index()
        {
			ViewData["typesOfTimeOff"] = new string[] { "Vacation", "Personal Emergency", "Appointment" };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
<<<<<<< HEAD
        public ActionResult Index([Bind(Include = "StartDate,EndDate,Reason,Type")] TimeOffRequest timeOffRequest)
=======
		[VerifyLogin]
		public ActionResult Index([Bind(Include = "StartDate,EndDate,Reason,Type")] TimeOffRequest timeOffRequest)
>>>>>>> 05a58a47ea21009b6645c7f1a461045f2bfef14b
        {
            if ((timeOffRequest.StartDate < DateTime.Now)
                || (timeOffRequest.EndDate < DateTime.Now)
                || (timeOffRequest.StartDate > timeOffRequest.EndDate))
			{
				ViewData["typesOfTimeOff"] = new string[] { "Vacation", "Personal Emergency", "Appointment" };
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
                    db.Employees.Find(sessionEmployee).TimeOffRequests.Add(timeOffRequest);
                    db.SaveChanges();
                } catch (Exception e) {
                    return RedirectToAction("Failure");
                }
                return RedirectToAction("Success");
            }

			ViewData["typesOfTimeOff"] = new string[] { "Vacation", "Personal Emergency", "Appointment" };
			return View(timeOffRequest);
		}
		[VerifyLogin]
		public ActionResult Success()
        {
            return View();
		}
		[VerifyLogin]
		public ActionResult Failure()
        {
            return View();
		}
		[VerifyLogin]
		public ActionResult AdminApproval()
        {
            Employee currentEmployee = db.Employees.Include(e => e.Position).Where(e => e.EmployeeId == sessionEmployee).FirstOrDefault();
            return View(db.TimeOffRequests
                .Include(e => e.Employee)
                .Include(t => t.Employee.Position)
                .Where(t => t.Status == "No")
                .Where(t => t.Employee.Position.Rank < currentEmployee.Position.Rank)
                .ToList());
<<<<<<< HEAD
        }
        public ActionResult Accept(int id)
=======
		}
		[VerifyLogin]
		public ActionResult Accept(int id)
>>>>>>> 05a58a47ea21009b6645c7f1a461045f2bfef14b
        {
            TimeOffRequest req = db.TimeOffRequests.Include(e => e.Employee).Where(e => e.TimeOffRequestId == id).FirstOrDefault();
            SmtpClient client = new SmtpClient("smtp.live.com", 25);
            client.Credentials = new System.Net.NetworkCredential("vpnprez@hotmail.com", "dudethatko1");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            MailMessage msg = new MailMessage("vpnprez@hotmail.com", req.Employee.Email, "Accepted", "Congrats bud");
            client.Send(msg);
            req.Status = "Yes";
            db.Entry(req).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AdminApproval");
        }
        [HttpPost, ActionName("Accept")]
        [ValidateAntiForgeryToken]
		[VerifyLogin]
		public ActionResult AcceptDelete(int id)
        {
            TimeOffRequest req = db.TimeOffRequests.Find(id);
            //db.TimeOffRequests.Remove(req);
            db.SaveChanges();
            return RedirectToAction("AdminApproval");
		}
		[VerifyLogin]
		public ActionResult Decline(int id)
        {
            Employee emp = null;
            TimeOffRequest req = db.TimeOffRequests.Find(id);

            foreach (Employee e in db.Employees)
            {
                foreach (TimeOffRequest tmp in e.TimeOffRequests)
                {
                    if (tmp == req)
                    {
                        emp = e;
                        string email = emp.Email;
                        SmtpClient client = new SmtpClient("smtp.live.com", 25);
                        client.Credentials = new System.Net.NetworkCredential("vpnprez@hotmail.com", "dudethatko1");
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.EnableSsl = true;
                        MailMessage msg = new MailMessage("vpnprez@hotmail.com", email, "Declined", "Sorry bud");
                        client.Send(msg);
                        return View(emp);
                    }
                }
            }
            return RedirectToAction("AdminApproval");
        }
        [HttpPost, ActionName("Decline")]
        [ValidateAntiForgeryToken]
		[VerifyLogin]
		public ActionResult DeclineDelete(int id)
        {
            TimeOffRequest req = db.TimeOffRequests.Find(id);
            db.TimeOffRequests.Remove(req);
            db.SaveChanges();
            return RedirectToAction("AdminApproval");
        }
    }
}
