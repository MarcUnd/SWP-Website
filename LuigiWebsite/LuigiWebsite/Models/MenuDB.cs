using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Models {
    public class MenuDB {

        private int _menuId;
        private decimal _preis;

        public int MenuId {
            get { return this._menuId; }
            set { if (value >= 1) {
                    this._menuId = value;
                }
            }
        }

        public decimal Preis {
            get { return this._preis; }
            set { if(value >= 1) {
                    this._preis = value;
                }
            }
        }

        public String Name { get; set; }
        public String Zutaten { get; set; }

        
    }
}
