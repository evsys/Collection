using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Collection.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
