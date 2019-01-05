using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MVC_Practice.Models.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public int Year { get; set; }
        public ApplicationUser()
        {
        }
    }
}