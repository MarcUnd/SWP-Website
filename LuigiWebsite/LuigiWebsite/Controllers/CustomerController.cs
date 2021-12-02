using LuigiWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Controllers {
    public class CustomerController : Controller {
        public IActionResult Reservation() {
            return View();
        }
        [HttpPost]
        public IActionResult Resgistration(user userData) {
            if(userData == null) {
                return RedirectToAction("Registration");
            }
            return View();
        }
        public IActionResult Login() {
            return View();
        }
    }
}
