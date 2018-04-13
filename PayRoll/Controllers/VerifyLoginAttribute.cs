using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PayRoll.Controllers
{
	public class VerifyLoginAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			HttpContext ctx = HttpContext.Current;

			// check  sessions here
			if (HttpContext.Current.Session["EmployeeId"] == null)
			{
				filterContext.Result = new RedirectResult("~/Employees/Login");
				return;
			}

			//base.OnActionExecuting(filterContext);
		}
	}
}