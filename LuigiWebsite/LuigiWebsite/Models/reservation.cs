using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Models {
    public class reservation {

        private int _resId;

        public int ResId {
            get { return this._resId; }
            set { if (value >= 1) {
                    this._resId = value;
                }
            } 
        }

        public string nachname { get; set; }

        public string email { get; set; }

        public DateTime date { get; set; }

        public DateTime uhrzeit { get; set; }
    }
}
