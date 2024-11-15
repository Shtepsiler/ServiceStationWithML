using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Seeders;
namespace PARTS.DAL.Data.Configurations
{
    public class PartConfiguration : IEntityTypeConfiguration<Part>
    {
        public void Configure(EntityTypeBuilder<Part> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(pi => pi.PartImage).WithOne(p => p.Part).HasForeignKey<PartImage>(ci => ci.PartId); // Ось цей рядок визначає зовнішній ключ;

            builder.HasOne(p => p.Category)
                              .WithMany(c => c.Parts)
                              .HasForeignKey(p => p.CategoryId);
        /*    PartSeeder brandSeeder = new PartSeeder();
            brandSeeder.Seed(builder);
*/
        }
    }
}
