namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectStage")]
    public partial class ProjectStage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectStage()
        {
            StorageShipments = new HashSet<StorageShipment>();
        }

        public int projectStageID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? startDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? endDate { get; set; }

        public int? projectID { get; set; }

        public int? stageID { get; set; }

        public virtual Project Project { get; set; }

        public virtual Stage Stage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorageShipment> StorageShipments { get; set; }
    }
}
