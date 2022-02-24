using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Models {
    public class Message {

        public string Header { set; get; }

        public string MessageText { set; get; }

        public string Solution { set; get; }


        public Message() : this("", "", "") { }

        public Message(string header, string message) : this(header, message, "") { }

        public Message(string header, string message, string solution) {
            this.Header = header;
            this.MessageText = message;
            this.Solution = solution;
        }
    }
}
