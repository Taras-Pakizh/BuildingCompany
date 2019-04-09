namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DeliveryOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryOrder()
        {
            DeliverysContents = new HashSet<DeliverysContent>();
        }

        public int deliveryOrderID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? deliveryOrderDate { get; set; }

        public double? sumPrice { get; set; }

        public int? supplierID { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliverysContent> DeliverysContents { get; set; }
    }
}
