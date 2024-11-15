using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Seeders;
namespace PARTS.DAL.Data.Configurations
{
    public class CategoryImageConfiguration : IEntityTypeConfiguration<CategoryImage>
    {
        public void Configure(EntityTypeBuilder<CategoryImage> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(ci => ci.Category).WithOne(p => p.CategoryImage).HasForeignKey<CategoryImage>(ci => ci.CategoryId); // Ось цей рядок визначає зовнішній ключ


/*            CategoryImageSeeder brandSeeder = new CategoryImageSeeder();
            brandSeeder.Seed(builder);*/

        }
    }
}
