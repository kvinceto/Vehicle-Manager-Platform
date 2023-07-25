namespace Vmp.Data
{
    using System.Reflection;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Vmp.Data.Models;

    public class VehicleManagerDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public VehicleManagerDbContext(DbContextOptions<VehicleManagerDbContext> options)
            : base(options)
        {
        }

        public DbSet<CostCenter> CostCenters { get; set; } = null!;

        public DbSet<Owner> Owners { get; set; } = null!;

        public DbSet<Vehicle> Vehicles { get; set; } = null!;

        public DbSet<Waybill> Waybills { get; set; } = null!;

        public DbSet<DateCheck> DateChecks { get; set; } = null!;

        public DbSet<MileageCheck> MileageChecks { get; set; } = null!;

        public DbSet<TaskModel> Tasks { get; set; } = null!;

        public DbSet<ApplicationUser> AspNetUsers { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(VehicleManagerDbContext)) ??
                                     Assembly.GetExecutingAssembly();
            builder.ApplyConfigurationsFromAssembly(configAssembly);

            base.OnModelCreating(builder);
        }
    }
}