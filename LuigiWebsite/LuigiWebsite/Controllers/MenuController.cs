using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Controllers {
    public class MenuController : Controller {
        public IActionResult Menu() {
            return View();
        }
    }
}
