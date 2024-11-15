using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Seeders;
namespace PARTS.DAL.Data.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(v => v.Make).WithMany(m => m.Vehicles);
            builder.HasOne(v => v.Model).WithMany(mdl => mdl.Vehicles);
            builder.HasOne(v => v.SubModel).WithMany(sm => sm.Vehicles);
            builder.HasOne(v => v.Engine).WithMany(e => e.Vehicles);

/*            VehicleSeeder brandSeeder = new VehicleSeeder();
            brandSeeder.Seed(builder);*/


        }
    }
}
