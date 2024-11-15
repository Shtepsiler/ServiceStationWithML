using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Seeders;
namespace PARTS.DAL.Data.Configurations
{
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(mdl => mdl.Make).WithMany(m => m.Models);
            /*  builder.HasMany(mdl => mdl.Vehicles).WithOne(v => v.Model);
              builder.HasMany(mdl => mdl.SubModels).WithOne(sm => sm.Model);*/

/*            ModelSeeder brandSeeder = new ModelSeeder();
            brandSeeder.Seed(builder);*/


        }
    }
}
