using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAVTWebapi.Models {
    public class LoginModel {

        public long Serno { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
