using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WbAppMVC.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth


        [HttpPost]
        public ActionResult Login()
        {
            var Data = "You Are Logged In Succsessfully";
            return Json(Data);
        }

        [HttpGet]
        public ActionResult GtoToLogin()
        {
            return View("Login");
        }
    }
}