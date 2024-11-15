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
    public class SpecialisationConfiguration : IEntityTypeConfiguration<Specialisation>
    {
        public void Configure(EntityTypeBuilder<Specialisation> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasMany(p => p.Mechanics).WithOne(p => p.Specialisation);

        }
    }
}
