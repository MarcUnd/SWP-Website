using LuigiWebsite.Models;
using LuigiWebsite.Models.DB;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Registration(user userData) {
            if (ModelState.IsValid) {
                try {
                    await _rep.ConnectAsync();
                    if (await _rep.InsertAsync(userData)) {
                        //MessageView aufrufen
                        return RedirectToAction("Login");
                    } else {
                        return View("_Message", new Message("Registration", "Sie haben sich NICHT erfolgreich registriert!!",
                            "Bitte versuchen sie es spaeter erneut!"));
                    }
                    //DbException, Basisklasse der Datenbank-Exception
                } catch (DbException) {
                    return View("_Message", new Message("Registration", "Datenbankfehler!",
                           "Bitte versuchen sie es spaeter erneut!"));
                } finally {
                    await _rep.Disconnect();
                }
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
        public async Task<IActionResult> Login(String email, String password) {
            if (ModelState.IsValid) {
                try {
                    await _rep.ConnectAsync();
                    if (await _rep.isUserAsync(email, password)) {
                        HttpContext.Session.SetInt32("login",1);
                        return RedirectToPage("");
                    } else {
                        return View("_Message", new Message("Login", "Sie haben sich NICHT erfolgreich eingelogt!!",
                            "Bitte versuchen sie es sp�ter erneut!"));
                    }
                    //DbException, Basisklasse der Datenbank-Exception
                } catch (DbException) {
                    return View("_Message", new Message("Login", "Datenbankfehler!",
                           "Bitte versuchen sie es sp�ter erneut!"));
                } finally {
                    await _rep.Disconnect();
                }
            }
            return RedirectToAction("registration");
        }
        [HttpGet]
        public IActionResult Login() {
            return View();
        }


        [HttpGet]
        public IActionResult Logout() {
            HttpContext.Session.SetInt32("login", 0);
            return View("_Message", new Message("Logout", "Sie haben sich erflogreich abgemeldet!"));  
        }
    } 
}    