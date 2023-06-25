namespace Vmp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Vmp.Data.Models;

    public class MileageCheckEntityConfiguration : IEntityTypeConfiguration<MileageCheck>
    {
        public void Configure(EntityTypeBuilder<MileageCheck> builder)
        {
            builder.HasOne(mc => mc.Vehicle)
                .WithMany(v => v.MileageChecks)
                .HasForeignKey(mc => mc.VehicleNumber)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(mc => mc.User)
                .WithMany(u => u.MileageChecks)
                .HasForeignKey(mc => mc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
