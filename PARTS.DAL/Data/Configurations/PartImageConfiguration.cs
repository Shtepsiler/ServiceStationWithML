using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Seeders;
namespace PARTS.DAL.Data.Configurations
{
    public class PartImageConfiguration : IEntityTypeConfiguration<PartImage>
    {
        public void Configure(EntityTypeBuilder<PartImage> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(pi => pi.Part).WithOne(p => p.PartImage).HasForeignKey<PartImage>(ci => ci.PartId); // Ось цей рядок визначає зовнішній ключ;;

/*            PartImageSeeder brandSeeder = new PartImageSeeder();
            brandSeeder.Seed(builder);*/


        }
    }
}
