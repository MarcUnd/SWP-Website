using LuigiWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Controllers {
    public class MenuController : Controller {
        public IActionResult Menu() {
            return View(MenuFromDB());
        }

        private List<MenuDB> MenuFromDB() {
            return new List<MenuDB> {
                new MenuDB{MenuId = 1, Preis = 6.90m, Name = "Pizza Margherita", Zutaten = "Tomaten, Mozarella, Basilikum"},
                new MenuDB{MenuId = 2, Preis = 8.90m, Name = "Pizza Prosciuto", Zutaten = "Tomaten, Mozarella, Schinken"},
                new MenuDB{MenuId = 3, Preis = 9.90m, Name = "Pizza Prosciuto E Funghi", Zutaten = "Tomaten, Mozarella, Schinken, Champignons"},
                new MenuDB{MenuId = 4, Preis = 9.90m, Name = "Pizza Tonno", Zutaten = "Tomaten, Mozarella, Thunfisch, Zwiebeln"},
                new MenuDB{MenuId = 5, Preis = 10.90m, Name = "Pizza Rustica", Zutaten = "Tomaten, Mozarella, Oliven, Zwiebeln"},
                new MenuDB{MenuId = 6, Preis = 10.90m, Name = "Pizza Hawaii", Zutaten = "Tomaten, Mozarella, Schinken, Ananas"},
                new MenuDB{MenuId = 7, Preis = 9.90m, Name = "Pizza Funghi", Zutaten = "Tomaten, Mozarella, Champignons"},
                new MenuDB{MenuId = 8, Preis = 8.90m, Name = "Pizza Rucola", Zutaten = "Tomaten, Mozarella, Rucola, Parmesan, Knoblauch"},
                new MenuDB{MenuId = 9, Preis = 10.90m, Name = "Pizza Vegetaria", Zutaten = "Tomaten, Mozarella, gegrilltes Gemüse, Champignons, Rucola, Knoblauch"},
                new MenuDB{MenuId = 10, Preis = 11.90m, Name = "Pizza Calzone", Zutaten = "Tomaten, Mozarella, Schinken, Champignons, Salami"},
                new MenuDB{MenuId = 11, Preis = 8.90m, Name = "Pizza Salami", Zutaten = "Tomaten, Mozarella, Salami"}
            };
        }
    }
}
