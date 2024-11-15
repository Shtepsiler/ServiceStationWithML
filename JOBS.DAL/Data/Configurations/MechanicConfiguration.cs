using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOBS.DAL.Entities;

namespace JOBS.DAL.Data.Configurations
{
    public class MechanicConfiguration : IEntityTypeConfiguration<Mechanic>
    {
        public void Configure(EntityTypeBuilder<Mechanic> builder)
        {
            //    builder.Property(p => p.Id).UseIdentityColumn();
            builder.HasKey(p => p.MechanicId);
            builder.HasMany(p => p.MechanicsTasks).WithOne(p => p.Mechanic);

        }
    }
}
