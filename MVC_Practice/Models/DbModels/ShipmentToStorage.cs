namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ShipmentToStorage")]
    public partial class ShipmentToStorage
    {
        public int shipmentToStorageID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? shipmentDate { get; set; }

        public double? resourceAmount { get; set; }

        public int? contentID { get; set; }

        public int? resourceStorageID { get; set; }

        public virtual DeliverysContent DeliverysContent { get; set; }

        public virtual Resources_Storages Resources_Storages { get; set; }
    }
}
