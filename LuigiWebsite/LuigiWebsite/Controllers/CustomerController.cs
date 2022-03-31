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
        public async Task<IActionResult> Reservation(reservation reservationData) {
            if (ModelState.IsValid) {
                try {
                    await _rep.ConnectAsync();
                    if (await _rep.InsertResAsync(reservationData)) {
                        //MessageView aufrufen
                        return View("_Message", new Message("Reservierung", "Sie haben erfolgreich reserviert!"));
                    } else {
                        return View("_Message", new Message("Reservierung", "Sie haben NICHT erfolgreich reserviert!!",
                            "Bitte versuchen sie es sp�ter erneut!"));
                    }
                    //DbException, Basisklasse der Datenbank-Exception
                }catch (DbException) {
                    return View("_Message", new Message("Reservierung", "Datenbankfehler!",
                           "Versuchen Sie es spaeter erneut!"));
                } finally {
                    await _rep.DisconnectAsync();
                }
            }
            return RedirectToAction("reservation");
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
                        return RedirectToAction("Login", userData);
                    } else {
                        return View("_Message", new Message("Registration", "Sie haben sich NICHT erfolgreich registriert!!",
                            "Bitte versuchen sie es spaeter erneut!"));
                    }
                    //DbException, Basisklasse der Datenbank-Exception
                } catch (DbException) {
                    return View("_Message", new Message("Registration", "Datenbankfehler!",
                           "Bitte versuchen sie es spaeter erneut!"));
                } finally {
                    await _rep.DisconnectAsync();
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
                        return RedirectToAction("index", "home");
                    } else {
                        return View("_Message", new Message("Login", "Sie haben sich NICHT erfolgreich eingelogt!!",
                            "Bitte versuchen sie es spaeter erneut!"));
                    }
                    //DbException, Basisklasse der Datenbank-Exception
                } catch (DbException) {
                    return View("_Message", new Message("Login", "Datenbankfehler!",
                           "Bitte versuchen sie es spaeter erneut!"));
                } finally {
                    await _rep.DisconnectAsync();
                }
            }
            return RedirectToAction("registration");
        }
        

        [HttpGet]
        public IActionResult Login(user userData) {
            user userEmail = new user(userData.email);
            return View(userEmail);
        }


        [HttpGet]
        public IActionResult Logout() {
            HttpContext.Session.SetInt32("login", 0);
            return View("_Message", new Message("Logout", "Sie haben sich erflogreich abgemeldet!"));  
        }
    } 
}    