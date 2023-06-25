namespace Vmp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Vmp.Data.Models;

    public class WaybillEntityConfiguration : IEntityTypeConfiguration<Waybill>
    {
        public void Configure(EntityTypeBuilder<Waybill> builder)
        {
            builder.Property(v => v.FuelQuantityStart).HasColumnType("decimal(14,3)");
            builder.Property(v => v.FuelQuantityEnd).HasColumnType("decimal(14,3)");
            builder.Property(v => v.FuelConsumed).HasColumnType("decimal(14,3)");
            builder.Property(v => v.FuelLoaded).HasColumnType("decimal(14,3)");


            builder.HasOne(w => w.Vehicle)
                .WithMany(v => v.Waybills)
                .HasForeignKey(w => w.VehicleNumber)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(w => w.CostCenter)
                .WithMany(c => c.Waybills)
                .HasForeignKey(w => w.CostCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(w => w.User)
                .WithMany(u => u.Waybills)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
