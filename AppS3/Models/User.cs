using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public DateTime? DoB { get; set; }
    }
}
