using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RavenMVCProducts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "RavenDB Product TestHarness";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
