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
<<<<<<< HEAD

        // GET: ChangePassword
        public ActionResult Index()
=======
		private string sessionEmployee = System.Web.HttpContext.Current.Session["EmployeeId"] as String;

		// GET: ChangePassword
		[VerifyLogin]
		public ActionResult Index()
>>>>>>> 05a58a47ea21009b6645c7f1a461045f2bfef14b
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
<<<<<<< HEAD
=======
		[VerifyLogin]
>>>>>>> 05a58a47ea21009b6645c7f1a461045f2bfef14b
		public ActionResult Index(FormCollection forms)
		{
			string newPassword = forms["newPassword"];
			string confirmPassword = forms["confirmPassword"];
<<<<<<< HEAD
			Employee employee = db.Employees.Find("a00828729");
=======
			Employee employee = db.Employees.Find(sessionEmployee);
>>>>>>> 05a58a47ea21009b6645c7f1a461045f2bfef14b
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
<<<<<<< HEAD
=======
		[VerifyLogin]
>>>>>>> 05a58a47ea21009b6645c7f1a461045f2bfef14b
		public ActionResult Success()
		{
			return View();
		}
	}
}
