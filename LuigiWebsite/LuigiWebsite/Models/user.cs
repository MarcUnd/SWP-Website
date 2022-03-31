using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Models {
    public class user {

        private int _customerId;
   
        public int CustomerId {
            get { return this._customerId; }
            set {
                if (value >= 1) {
                    this._customerId = value;
                }
            }
        }
        public string email { get; set; }
        public string vorname { get; set; }
        public string nachname { get; set; }
        public string password { get; set; }
        public DateTime BirthDate { get; set; }

        public user(string email) {
            this.email = email;
        }

        public user() { }
        
    }
}
