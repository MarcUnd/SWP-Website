using LuigiWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Controllers {
    public class CustomerController : Controller {

        [HttpGet]
        public IActionResult Reservation() {
            return View();
        }

        [HttpPost]
        public IActionResult Reservation(reservation reservationData) {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(user userData) {
            if(userData == null) {
                return RedirectToAction("Registration");
            }
            ValidateRegistrationData(userData);
            if (ModelState.IsValid) {
                return RedirectToAction("Login");
            }
            return View(userData);
        }

        [HttpGet]
        public IActionResult Registration() {
            return View();
        }


        private void ValidateRegistrationData(user u) {
           if (u == null) {
                return;
            }
           if(u.vorname == null) {
                ModelState.AddModelError("vorname", "Bitte tragen sie einen Vornamen ein!");
            }
            if(u.nachname == null) {
                ModelState.AddModelError("nachname", "Bitte tragen sie einen Nachnamen ein!");
            }
            if(u.password == null || u.password.Length < 8) {
                ModelState.AddModelError("password", "Ihr Passwort muss mindesten acht Zeichen lang sein!");
            }
            if (u.BirthDate > DateTime.Now) {
                ModelState.AddModelError("BirthDate", "Das Geburstdatum kann nicht in der Zukunft liegen!");
            }
            if(u.email == null) {
                ModelState.AddModelError("email", "Bitte tragen sie eine Email-Addresse ein!");

            }


        }

        public IActionResult Login() {
            return View();
        }
    }
}
