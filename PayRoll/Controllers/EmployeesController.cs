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
    public class EmployeesController : Controller
    {
        private PayrollDbContext db = new PayrollDbContext();
        private string sessionEmployee = System.Web.HttpContext.Current.Session["EmployeeId"] as String;

        // GET: Employees
        public ActionResult Index()
        {
            Employee currentEmployee = db.Employees.Include(e => e.Position).Where(e => e.EmployeeId == sessionEmployee).FirstOrDefault();
            return View(db.Employees.Include(e => e.Position).Where(e => e.Position.Rank < currentEmployee.Position.Rank));
        }

        // GET: Employees/Details/5
        public ActionResult Details(string id)
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

        // GET: Employees/Create
        public ActionResult Create()
        {
            Employee currentEmployee = db.Employees.Include(e => e.Position).Where(e => e.EmployeeId == sessionEmployee).FirstOrDefault();
            ViewData["positions"] = db.Positions.Where(p => p.Rank < currentEmployee.Position.Rank).ToArray();
			return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Password,FName,LName,Address,Email,FullOrPartTime,Seniority,DepartmentType,HourlyRate")] Employee employee)
        {
			employee.EmployeeId = generateEmployeeId();
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
				db.Positions.Find(Request.Form.Get("Position")).Employees.Add(employee);
				db.SaveChanges();
				return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
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
            Employee currentEmployee = db.Employees.Include(e => e.Position).Where(e => e.EmployeeId == sessionEmployee).FirstOrDefault();
            ViewData["positions"] = db.Positions.Where(p => p.Rank < currentEmployee.Position.Rank).ToArray();
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,Password,FName,LName,Address,Email,FullOrPartTime,Seniority,DepartmentType")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

		private string generateEmployeeId()
		{
			Random rnd = new Random();
			string result = "a00";
			for (int i = 0; i < 6; i++)
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
                return RedirectToAction("Login","Employees");
            }

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}


