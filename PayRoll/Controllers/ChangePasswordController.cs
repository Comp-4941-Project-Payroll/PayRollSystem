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
    public class ChangePasswordController : Controller
    {
        private PayrollDbContext db = new PayrollDbContext();

        // GET: ChangePassword
        public ActionResult Index()
		{
			//if (id == null)
			//{
			//	return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			//}
			Employee employee = db.Employees.Find("a00828729");
			if (employee == null)
			{
				return HttpNotFound();
			}
			return View(employee);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Index(FormCollection forms)
		{
			string newPassword = forms["newPassword"];
			string confirmPassword = forms["confirmPassword"];
			Employee employee = db.Employees.Find("a00828729");
			if (newPassword != confirmPassword)
			{
				ModelState.AddModelError("NewPassword", "Passwords do not match");
				return View(employee);
			}
			try
			{
				employee.Password = confirmPassword;
				db.Entry(employee).State = EntityState.Modified;
				db.SaveChanges();
			}
			catch (System.Data.Entity.Validation.DbEntityValidationException e)
			{
				ModelState.AddModelError("NewPassword", e.EntityValidationErrors.ElementAt(0).ValidationErrors.ElementAt(0).ErrorMessage);
				employee = db.Employees.Find("a00828729");
			}
			return View(employee);
		}
    }
}
