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
		private string sessionEmployee = System.Web.HttpContext.Current.Session["EmployeeId"] as String;

		// GET: ChangePassword
		[VerifyLogin]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[VerifyLogin]
		public ActionResult Index(FormCollection forms)
		{
			string newPassword = forms["newPassword"];
			string confirmPassword = forms["confirmPassword"];
			Employee employee = db.Employees.Find(sessionEmployee);
			string originalPassword = employee.Password;
			if (newPassword != confirmPassword)
			{
				ModelState.AddModelError("NewPassword", "Passwords do not match");
				return View();
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
				employee.Password = originalPassword;
				return View();
			}
			return RedirectToAction("Success");
		}
		[VerifyLogin]
		public ActionResult Success()
		{
			return View();
		}
	}
}
