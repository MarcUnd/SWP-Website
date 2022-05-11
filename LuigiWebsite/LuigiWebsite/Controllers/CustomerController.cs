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
        public async Task<IActionResult> Reservation() {
            reservation r = new reservation();
            if (HttpContext.Session.GetInt32("login") == 1) {
                if (ModelState.IsValid) {
                    try {
                        await _rep.ConnectAsync();

                        Task<user> u = _rep.getUserByEmailAsync(HttpContext.Session.GetString("email"));

                        r.email = u.Result.email;
                        r.nachname = u.Result.nachname;
                        //r.email = HttpContext.Session.GetString("email");
                    } catch (DbException) {
                        return View("_Message", new Message("Reservierung", "Datenbankfehler!",
                               "Versuchen Sie es spaeter erneut!"));
                    } finally {
                        await _rep.DisconnectAsync();
                    }
                }
            }
            return View(r);
        }

        [HttpPost]
        public async Task<IActionResult> Reservation(reservation reservationData) {
            if(reservationData == null) {
                return View();
            }
            if (ModelState.IsValid) {
                try {
                    await _rep.ConnectAsync();
                    
                    DateTime dat = reservationData.date.Date.Add(reservationData.uhrzeit.TimeOfDay);
                    if (await _rep.ResValidAsync(dat)) {
                        if (await _rep.InsertResAsync(reservationData)) {
                            return View("_Message", new Message("Reservierung", "Sie haben erfolgreich reserviert!"));
                        } else {
                            return View("_Message", new Message("Reservierung", "Sie haben NICHT reserviert!!", "versuchen sie es sp�ter erneut!"));
                        }
                    } else {
                        //return View(reservationData);
                        return View("_Message", new Message("Reservierung", "Zu dieser Zeit ist keine Reservierung verfügbar!"));
                    }


                }
                catch (DbException) {
                return View("_Message", new Message("Reservierung", "Datenbankfehler!",
                       "Versuchen Sie es spaeter erneut!"));
            } finally {
                await _rep.DisconnectAsync();
            }
        }
            return RedirectToAction("reservation");
    }
        public async Task<IActionResult> MyReservations() {
            try {
                await _rep.ConnectAsync();
                return View(await _rep.getReservationsByEmailAsync(HttpContext.Session.GetString("email")));
            } catch (DbException) {
                return View("_Message", new Message("Benutzer", "Datenbankfehler", "Bitte versuchen Sie es später erneut!"));
            } finally {
                await _rep.DisconnectAsync();
            }
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
        }

        [HttpPost]
        public async Task<IActionResult> Registration(user userData) {
            if (ModelState.IsValid) {
                try {
                    await _rep.ConnectAsync();
                    if (await _rep.InsertAsync(userData)) {
                        
                        return RedirectToAction("Login", userData);
                    } else {
                            return View("_Message", new Message("Registration", "Ein User mit dieser Email ist bereits vorhanden!!!",
                            "Geben sie eine andere Email Adresse ein!"));
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

        
        public IActionResult CheckEmailAsync(string email) {
            // try {
                /*await _rep.ConnectAsync();
                if (await _rep.verifyUserByEmailAsync(email)) {
                */
                    return new JsonResult(true);
                //} else {
                //    return new JsonResult(false);
                //}
            //} catch (DbException) {
            //    return View("_Message", new Message("Registration", "Datenbankfehler!",
            //               "Bitte versuchen sie es spaeter erneut!"));
            //}
        }
        public IActionResult Delete(int id) {
            try {
                _rep.ConnectAsync();
                _rep.Delete(id);
                return View("_Message", new Message("Benutzer", "Benutzer wurde  gelöscht"));

            } catch (DbException) {
                return View("_Message", new Message("Benutzer", "Benutzer wurde nicht gelöscht", "Bitte versuchen Sie es später erneut!"));
            } finally {
                _rep.DisconnectAsync();
            }

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
                        HttpContext.Session.SetInt32("login", 1);
                        HttpContext.Session.SetString("email", email);
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