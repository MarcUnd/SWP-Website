using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Models {
    public class reservation {

        private int _resId;

        public int ResId { 
            get { return this._resId}
            set { if (value >= 1) {
                    this._resId = value;
                }
            } 
        }

        public string Nachname { get; set; }

        public DateTime Date { get; set; }

        public int Number { get; set; }
    }
}
