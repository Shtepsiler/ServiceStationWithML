using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Seeders;
namespace PARTS.DAL.Data.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(p => p.Id);
            // builder.HasMany(p => p.Parts).WithOne(p => p.Brand);

/*
            BrandSeeder brandSeeder = new BrandSeeder();
            brandSeeder.Seed(builder);*/
        }
    }
}
