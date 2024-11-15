using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Seeders;
namespace PARTS.DAL.Data.Configurations
{
    public class MakeConfiguration : IEntityTypeConfiguration<Make>
    {
        public void Configure(EntityTypeBuilder<Make> builder)
        {
            builder.HasKey(p => p.Id);

            /*   builder.HasMany(m => m.Models).WithOne(mdl => mdl.Make);
               builder.HasMany(m => m.Engines).WithOne(eng => eng.Make);
               builder.HasMany(m => m.Vehicles).WithOne(v => v.Make);*/

/*            MakeSeeder brandSeeder = new MakeSeeder();
            brandSeeder.Seed(builder);*/


        }
    }
}
