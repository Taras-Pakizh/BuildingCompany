namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StorageShipment")]
    public partial class StorageShipment
    {
        public int storageShipmentID { get; set; }

        public double? resourceAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? outDate { get; set; }

        public int? projectStageID { get; set; }

        public int? resourceStorageID { get; set; }

        public virtual ProjectStage ProjectStage { get; set; }

        public virtual Resources_Storages Resources_Storages { get; set; }
    }
}
