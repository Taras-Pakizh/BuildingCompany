namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vAdriftEmployee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int employeeID { get; set; }

        [StringLength(50)]
        public string firstname { get; set; }

        [StringLength(50)]
        public string lastname { get; set; }

        [Column(TypeName = "date")]
        public DateTime? birthday { get; set; }

        [StringLength(1)]
        public string gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? orderDate { get; set; }

        [StringLength(150)]
        public string orderDescription { get; set; }

        [StringLength(50)]
        public string positionName { get; set; }

        public double? salary { get; set; }

        [StringLength(50)]
        public string dname { get; set; }
    }
}
