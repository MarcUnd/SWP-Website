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
        public async Task<IActionResult> Menu() {
            try {
                await _rep.ConnectAsync();
                return View(await _rep.GetMenuAsync());
            } catch (DbException) {
                return View("_Message", new Message("Benutzer", "Datenbankfehler", "Bitte versuchen Sie es später erneut!"));
            } finally {
                await _rep.DisconnectAsync();
            }
        }
    }
}
