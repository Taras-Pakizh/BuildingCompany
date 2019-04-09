namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            ProjectStages = new HashSet<ProjectStage>();
            Departments = new HashSet<Department>();
        }

        public int projectID { get; set; }

        [StringLength(50)]
        public string pname { get; set; }

        public double? cost { get; set; }

        [Column(TypeName = "date")]
        public DateTime? workBeginning { get; set; }

        [Column(TypeName = "date")]
        public DateTime? deadline { get; set; }

        [StringLength(50)]
        public string addres { get; set; }

        public int? clientID { get; set; }

        public int? stateID { get; set; }

        public virtual Client Client { get; set; }

        public virtual State State { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectStage> ProjectStages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Department> Departments { get; set; }
    }
}
