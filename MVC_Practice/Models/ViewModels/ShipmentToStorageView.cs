using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace MVC_Practice.Models.ViewModels
{
    public class ShipmentToStorageView
    {
        [Required]
        public int? storageID { get; set; }

        [Required]
        public int? resourceID { get; set; }

        [Required]
        public DateTime? date { get; set; }

        [Required]
        public int? contentID { get; set; }

        [Required]
        public double? amount { get; set; }
    }
}