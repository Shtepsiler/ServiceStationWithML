using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Seeders;
namespace PARTS.DAL.Data.Configurations
{
    public class EngineConfiguration : IEntityTypeConfiguration<Engine>
    {
        public void Configure(EntityTypeBuilder<Engine> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(e => e.Make).WithMany(m => m.Engines);
            builder.HasOne(e => e.SubModel).WithMany(sm => sm.Engines);
            //  builder.HasMany(e => e.Vehicles).WithOne(sm => sm.Engine);

/*            EngineSeeder brandSeeder = new EngineSeeder();
            brandSeeder.Seed(builder);*/

        }
    }
}
