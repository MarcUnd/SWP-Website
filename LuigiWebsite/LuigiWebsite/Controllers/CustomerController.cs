using LuigiWebsite.Models;
using LuigiWebsite.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LuigiWebsite.Controllers {
    public class CustomerController : Controller {
        private IRepositoryDB _rep = new RepositoryDB();

        [HttpGet]
        public IActionResult Reservation() {
            return View();
        }

        [HttpPost]
        public IActionResult Reservation(reservation reservationData) {
            if (reservationData == null) {
                return RedirectToAction("reservation");
            }
            ValidateReservationData(reservationData);
            if (ModelState.IsValid) {
                return RedirectToAction("index", "Home");
            }
            return View(reservationData);
        }

        private void ValidateReservationData(reservation r) {
            if (r == null) {
                return;
            }
            if (r.nachname == null) {
                ModelState.AddModelError("nachnem", "Bitte tragen sie einen Nachnamen ein!");
            }
            if (r.email == null) {
                ModelState.AddModelError("email", "Bitte tragen sie einen Emailaddresse ein!");
            }
            if (r.number == null) {
                ModelState.AddModelError("number", "Bitte tragen sie eine richtige Telefonnummer ein!");
            }
            if (r.date < DateTime.Now.AddDays(-1)) {
                ModelState.AddModelError("date", "Das Datum kann nicht in der Vergangenheit liegen!");
            }
            if (r.uhrzeit == null) {
                ModelState.AddModelError("uhrzeit", "Bitte tragen sei eine Uhrzeit ein!");

            }
        }

        [HttpPost]
        public IActionResult Registration(user userData) {
            if (userData == null) {
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
            if (u.vorname == null) {
                ModelState.AddModelError("vorname", "Bitte tragen sie einen Vornamen ein!");
            }
            if (u.nachname == null) {
                ModelState.AddModelError("nachname", "Bitte tragen sie einen Nachnamen ein!");
            }
            if (u.password == null || u.password.Length < 8) {
                ModelState.AddModelError("password", "Ihr Passwort muss mindesten acht Zeichen lang sein!");
            }
            if (u.BirthDate > DateTime.Now || u.BirthDate.AddYears(16) > DateTime.Now) {
                ModelState.AddModelError("BirthDate", "Das Geburstdatum kann nicht in der Zukunft liegen!");
            }
            if (u.email == null) {
                ModelState.AddModelError("email", "Bitte tragen sie eine Email-Addresse ein!");

            }
        }

        [HttpPost]
        public IActionResult Login(String username, String password) {
            if (ModelState.IsValid) {
                try {
                    _rep.Connect();
                    if (_rep.isUser(username, password)) {
                        //MessageView aufrufen
                        return View("_Message", new Message("Login", "Sie haben sich erfolgreich eingelogt!"));
                    } else {
                        return View("_Message", new Message("Login", "Sie haben sich NICHT erfolgreich eingelogt!!",
                            "Bitte versuchen sie es später erneut!"));
                    }
                    //DbException, Basisklasse der Datenbank-Exception
                } catch (DbException) {
                    return View("_Message", new Message("Login", "Datenbankfehler!",
                           "Bitte versuchen sie es später erneut!"));
                } finally {
                    _rep.Disconnect();
                }
            }
            return View(username, password);
        }
        [HttpGet]
        public IActionResult Login() {
            return View();
        }
    }
}    