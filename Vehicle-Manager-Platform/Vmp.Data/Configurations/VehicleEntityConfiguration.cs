namespace Vmp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Vmp.Data.Models;

    public class VehicleEntityConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.Property(v => v.FuelQuantity).HasColumnType("decimal(14,3)");
            builder.Property(v => v.FuelCapacity).HasColumnType("decimal(14,3)");
            builder.Property(v => v.FuelCostRate).HasColumnType("decimal(14,3)");

            builder
                .HasOne(v => v.Owner)
                .WithMany(o => o.Vehicles)
                .HasForeignKey(v => v.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
