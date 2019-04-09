namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Resources_Storages
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Resources_Storages()
        {
            ShipmentToStorages = new HashSet<ShipmentToStorage>();
            StorageShipments = new HashSet<StorageShipment>();
        }

        [Key]
        public int resourceStorageID { get; set; }

        public double? resourceAmount { get; set; }

        public int? storageID { get; set; }

        public int? resourceID { get; set; }

        public virtual Resource Resource { get; set; }

        public virtual Storage Storage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShipmentToStorage> ShipmentToStorages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorageShipment> StorageShipments { get; set; }
    }
}
