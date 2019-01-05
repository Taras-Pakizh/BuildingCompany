namespace MVC_Practice.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyModels : DbContext
    {
        public MyModels()
            : base("name=MyModels")
        {
        }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<EmployeeOrder> EmployeeOrders { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasMany(e => e.Employees)
                .WithOptional(e => e.Department1)
                .HasForeignKey(e => e.department);

            modelBuilder.Entity<Employee>()
                .Property(e => e.gender)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.EmployeeOrders)
                .WithOptional(e => e.Employee1)
                .HasForeignKey(e => e.Employee);

            modelBuilder.Entity<OrderType>()
                .HasMany(e => e.EmployeeOrders)
                .WithOptional(e => e.OrderType1)
                .HasForeignKey(e => e.orderType);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.Employees)
                .WithOptional(e => e.Position1)
                .HasForeignKey(e => e.position);
        }
    }
}
