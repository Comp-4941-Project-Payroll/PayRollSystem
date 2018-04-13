using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PayRoll.Models;

namespace PayRoll.Controllers
{
    public class EmployeesController : Controller
    {
        private PayrollDbContext db = new PayrollDbContext();
        private string sessionEmployee = System.Web.HttpContext.Current.Session["EmployeeId"] as String;

		// GET: Employees
		[VerifyLogin]
		public ActionResult Index()
        {
            Employee currentEmployee = db.Employees.Include(e => e.Position).Where(e => e.EmployeeId == sessionEmployee).FirstOrDefault();

            return View(db.Employees.Include(e => e.Position).Include(s => s.Shift).Where(e => e.Position.Rank < currentEmployee.Position.Rank));
        }

		// GET: Employees/Details/5
		[VerifyLogin]
		public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Include(e => e.Position).Where(e=>e.EmployeeId == id).FirstOrDefault();
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

		// GET: Employees/Create
		[VerifyLogin]
		public ActionResult Create()
        {
            Employee currentEmployee = db.Employees.Include(e => e.Position).Where(e => e.EmployeeId == sessionEmployee).FirstOrDefault();
            ViewData["positions"] = db.Positions.Where(p => p.Rank < currentEmployee.Position.Rank).ToArray();
            ViewData["departmentTypes"] = new string[] { "Production", "Research and Development", "Purchasing", "Marketing", "Human Resources", "Accounting and Finance", "Executive" };
			ViewData["schedule"] = db.Schedules.ToArray();
			return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

		[VerifyLogin]
		public ActionResult Create([Bind(Include = "FName,LName,Address,Email,FullOrPartTime,Seniority,DepartmentType,HourlyRate")] Employee employee)
   {
			employee.EmployeeId = GenerateEmployeeId();
            employee.Password = GeneratePassword();
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
				db.Positions.Find(Request.Form.Get("Position")).Employees.Add(employee);
				db.SaveChanges();
                SmtpClient client = new SmtpClient("smtp.live.com", 25);
                client.Credentials = new System.Net.NetworkCredential("vpnprez@hotmail.com", "dudethatko1");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                MailMessage msg = new MailMessage("vpnprez@hotmail.com", employee.Email, "You've Been Hired", "Your Employee ID: " + employee.EmployeeId + "\nYour password: " + employee.Password);
                client.Send(msg);
                return RedirectToAction("Index");
            }

            ViewData["departmentTypes"] = new string[] { "Production", "Research and Development", "Purchasing", "Marketing", "Human Resources", "Accounting and Finance", "Executive" };
            return View(employee);
        }

		// GET: Employees/Edit/5
		[VerifyLogin]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            Employee currentEmployee = db.Employees.Include(e => e.Position).Include(e => e.Shift).Where(e => e.EmployeeId == sessionEmployee).FirstOrDefault();
            ViewData["positions"] = db.Positions.Where(p => p.Rank < currentEmployee.Position.Rank).ToArray();
            ViewData["departmentTypes"] = new string[] { "Production", "Research and Development", "Purchasing", "Marketing", "Human Resources", "Accounting and Finance", "Executive" };
            ViewData["schedules"] = db.Schedules.ToArray();
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,FName,LName,Address,Email,FullOrPartTime,Seniority,DepartmentType,Shift")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["departmentTypes"] = new string[] { "Production", "Research and Development", "Purchasing", "Marketing", "Human Resources", "Accounting and Finance", "Executive" };
            return View(employee);
        }

        // GET: Employees/Delete/5
        [VerifyLogin]
		public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[VerifyLogin]
		public ActionResult DeleteConfirmed(string id)
        {
			Employee employee = db.Employees.Find(id);
			db.Attendances.RemoveRange(db.Attendances.Where(e => e.Employee.EmployeeId == id));
			db.TimeOffRequests.RemoveRange(db.TimeOffRequests.Where(e => e.Employee.EmployeeId == id));
			db.Payrolls.RemoveRange(db.Payrolls.Where(e => e.Employee.EmployeeId == id));
			db.Entry(employee).State = EntityState.Modified;
			db.SaveChanges();
			db.Employees.Remove(employee);
			db.SaveChanges();
            return RedirectToAction("Index");
        }

		[VerifyLogin]
		public ActionResult Email(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
		[VerifyLogin]
		public ActionResult SendEmail()
        {
            Employee currentEmployee = db.Employees.Find(sessionEmployee);
            string EmployeeId = Request["EmployeeId"];
            string subject = Request["Subject"];
            string body = Request["Body"];
            Employee employee = db.Employees.Find(EmployeeId);
            SmtpClient client = new SmtpClient("smtp.live.com", 25);
            client.Credentials = new System.Net.NetworkCredential("vpnprez@hotmail.com", "dudethatko1");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            MailMessage msg = new MailMessage("vpnprez@hotmail.com", employee.Email, subject, body + "\nFrom: " + currentEmployee.FName + " " + currentEmployee.LName);
            client.Send(msg);
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

		private string GenerateEmployeeId()
		{
			Random rnd = new Random();
			string result = "a00";
			for (int i = 0; i < 6; i++)
			{
				result += rnd.Next(0, 10);
			}
			return result;
		}

        private string GeneratePassword()
        {
            Random rnd = new Random();
            string result = "";
            for (int i = 0; i < 8; i++)
            {
                result += rnd.Next(0, 10);
            }
            return result;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(String EmployeeId, String password)
        {
            //Email Refers to EmployeeId textbox in Login page.
                var myEmployee = db.Employees
                      .FirstOrDefault(u => u.EmployeeId == EmployeeId
                                   && u.Password == password);

            if (myEmployee != null)
            {
                Session["EmployeeId"] = db.Employees.Where(x => x.EmployeeId == myEmployee.EmployeeId).FirstOrDefault().EmployeeId;
                Session["Position"] = db.Employees.Include(e => e.Position).Where(y => y.EmployeeId == myEmployee.EmployeeId).FirstOrDefault().Position.PositionId;
                return RedirectToAction("Index", "PayrollManage");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login credentials.");
                //return RedirectToAction("Login","Employees");
                return View("Login");
            }

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult AddShift(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Employee employee = db.Employees.Find(id);
            ViewBag.id = id;
            //if (employee == null)
            //{
            //	return HttpNotFound();
            //}
            return View();
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("AddShift")]
        public ActionResult ShiftConfirm([Bind(Include = "Shift")] Employee employee, string id)
        //public ActionResult ShiftConfirm(string id)
        {
            Employee emp = db.Employees.Find(id);
            if (emp == null)
            {
                return RedirectToAction("AddShift");
            }
            string shiftid = Request.Form.Get("shiftid");
            Schedule sched = new Schedule();
            if ((employee.Shift.StartTime < DateTime.Now)
                || (employee.Shift.EndTime < DateTime.Now)
                || (employee.Shift.EndTime < employee.Shift.StartTime))
            {
                return RedirectToAction("AddShift");
            }
            else
            {
                sched.StartTime = employee.Shift.StartTime;
                sched.EndTime = employee.Shift.EndTime;
                sched.Employees.Add(emp);
                db.Schedules.Add(sched);
                db.SaveChanges();
                employee.Shift = sched;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}


