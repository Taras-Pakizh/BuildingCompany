namespace MVC_Practice.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EmployeeOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int orderID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? orderDate { get; set; }

        public int? Employee { get; set; }

        public int? orderType { get; set; }

        [StringLength(50)]
        public string orderDescription { get; set; }

        public virtual Employee Employee1 { get; set; }

        public virtual OrderType OrderType1 { get; set; }
    }
}
