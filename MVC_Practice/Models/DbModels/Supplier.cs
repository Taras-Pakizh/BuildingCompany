namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supplier()
        {
            DeliveryOrders = new HashSet<DeliveryOrderr>();
        }

        public int supplierID { get; set; }

        [StringLength(50)]
        public string supplierName { get; set; }

        [StringLength(13)]
        public string supplierPhoneNumber { get; set; }

        [StringLength(50)]
        public string supplierEmail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryOrderr> DeliveryOrders { get; set; }
    }
}
