namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vAllResource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int resourceStorageID { get; set; }

        [StringLength(50)]
        public string resourceName { get; set; }

        [StringLength(50)]
        public string storageAddres { get; set; }

        public double? resourceAmount { get; set; }
    }
}
