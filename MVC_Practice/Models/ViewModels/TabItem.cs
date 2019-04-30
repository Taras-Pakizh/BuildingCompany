using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Practice.Models.ViewModels
{
    public class TabItem
    {
        public string name { get; set; }

        public bool Selected { get; set; }

        public string href { get; set; }
    }
}