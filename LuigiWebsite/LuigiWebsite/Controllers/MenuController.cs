using LuigiWebsite.Models;
using LuigiWebsite.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Controllers {
    public class MenuController : Controller {

        private IRepositoryDB _rep = new RepositoryDB();
        public IActionResult Menu() {
            try {
                _rep.Connect();
                return View(_rep.getMenu());
            } catch (DbException) {
                return View("_Message", new Message("Benutzer", "Datenbankfehler", "Bitte versuchen Sie es später erneut!"));
            } finally {
                _rep.Disconnect();
            }
        }
    }
}
