namespace MVC_Practice.Models.DbModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DbModels : DbContext
    {
        public DbModels()
            : base("name=DbModels")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<DeliveryOrderr> DeliveryOrders { get; set; }
        public virtual DbSet<DeliverysContent> DeliverysContents { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<EmployeeOrder> EmployeeOrders { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectStage> ProjectStages { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Resources_Storages> Resources_Storages { get; set; }
        public virtual DbSet<ShipmentToStorage> ShipmentToStorages { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Storage> Storages { get; set; }
        public virtual DbSet<StorageShipment> StorageShipments { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<DetailedEmployee> DetailedEmployees { get; set; }
        public virtual DbSet<vAdriftEmployee> vAdriftEmployees { get; set; }
        public virtual DbSet<vAllResource> vAllResources { get; set; }
        public virtual DbSet<vDefaultEmployee> vDefaultEmployees { get; set; }
        public virtual DbSet<vVacationEmployee> vVacationEmployees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasMany(e => e.Projects)
                .WithMany(e => e.Departments)
                .Map(m => m.ToTable("Department_Project").MapLeftKey("departmentID").MapRightKey("projectID"));

            modelBuilder.Entity<Employee>()
                .Property(e => e.gender)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DetailedEmployee>()
                .Property(e => e.gender)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<vAdriftEmployee>()
                .Property(e => e.gender)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<vDefaultEmployee>()
                .Property(e => e.gender)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<vVacationEmployee>()
                .Property(e => e.gender)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
