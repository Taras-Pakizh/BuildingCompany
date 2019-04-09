namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EmployeeOrder
    {
        [Key]
        public int eOrderID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? orderDate { get; set; }

        [StringLength(150)]
        public string orderDescription { get; set; }

        public int? employeeID { get; set; }

        public int? orderTypeID { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual OrderType OrderType { get; set; }
    }
}
