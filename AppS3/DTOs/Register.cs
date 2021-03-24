using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.DTOs
{
    public class Register
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Roles { get; set; }
    }
}
