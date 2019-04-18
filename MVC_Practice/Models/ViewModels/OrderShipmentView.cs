using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace MVC_Practice.Models.ViewModels
{
    public class OrderShipmentView
    {
        [Required]
        public int? storageID { get; set; }

        [Required]
        public int? orderID { get; set; }

        [Required]
        public DateTime? date { get; set; }
    }
}