namespace Vmp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Vmp.Data.Models;


    public class DateCheckEntityConfiguration : IEntityTypeConfiguration<DateCheck>
    {
        public void Configure(EntityTypeBuilder<DateCheck> builder)
        {
            builder.HasOne(dc => dc.Vehicle)
                .WithMany(v => v.DateChecks)
                .HasForeignKey(dc => dc.VehicleNumber)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(dc => dc.User)
                .WithMany(u => u.DateChecks)
                .HasForeignKey(dc => dc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
