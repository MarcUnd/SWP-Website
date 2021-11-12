using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Controllers {
        //hello
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }

        public IActionResult Location() {
            return View();
        }

        public IActionResult Employees() {
            return View();
        }

        public IActionResult Impressum() {
            return View();
        }
    }
}
