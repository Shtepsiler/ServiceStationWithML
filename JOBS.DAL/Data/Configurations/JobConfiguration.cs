using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JOBS.DAL.Entities;

namespace JOBS.DAL.Data.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {

        public void Configure(EntityTypeBuilder<Job> builder)
        {
          //  builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.ManagerId).IsRequired(false);
            builder.Property(p => p.VehicleId);
            builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("Pending");
            builder.Property(p => p.ClientId);
            builder.Property(p => p.MechanicId).IsRequired(false);
            builder.Property(p => p.IssueDate);
            builder.Property(p => p.FinishDate).IsRequired(false);
            builder.Property(p => p.Description);
            builder.Property(p => p.Price).IsRequired(false);

            builder.HasKey(p => p.Id);

       

        }
    }
}
