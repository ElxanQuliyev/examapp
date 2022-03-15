using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizK101.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        // GET: Dashboard/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}