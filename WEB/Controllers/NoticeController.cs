using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class NoticeController : Controller
    {
        //
        // GET: /Notice/

        public ActionResult AuthenticationSuccess()
        {
            string url = System.Configuration.ConfigurationManager.ConnectionStrings["url"].ConnectionString;
            ViewBag.url = url;
            return View();
        }
        public ActionResult NOPermission()
        {
            return View();
        }
    }
}
