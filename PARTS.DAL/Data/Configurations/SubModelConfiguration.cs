using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Seeders;
namespace PARTS.DAL.Data.Configurations
{
    public class SubModelConfiguration : IEntityTypeConfiguration<SubModel>
    {
        public void Configure(EntityTypeBuilder<SubModel> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(sm => sm.Model).WithMany(m => m.SubModels);
            /* builder.HasMany(sm => sm.Vehicles).WithOne(v => v.SubModel);
             builder.HasMany(sm => sm.Engines).WithOne(e => e.SubModel);*/

/*            SubModelSeeder brandSeeder = new SubModelSeeder();
            brandSeeder.Seed(builder);*/


        }
    }
}
