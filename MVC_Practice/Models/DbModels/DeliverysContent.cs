namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DeliverysContent")]
    public partial class DeliverysContent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliverysContent()
        {
            ShipmentToStorages = new HashSet<ShipmentToStorage>();
        }

        [Key]
        public int contentID { get; set; }

        public double? contentAmount { get; set; }

        public double? resourcePrice { get; set; }

        public int? resourceID { get; set; }

        public int? deliveryOrderID { get; set; }

        public virtual DeliveryOrderr DeliveryOrder { get; set; }

        public virtual Resource Resource { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShipmentToStorage> ShipmentToStorages { get; set; }
    }
}
