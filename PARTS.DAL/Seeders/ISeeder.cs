using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PARTS.DAL.Seeders
{
    public interface ISeeder<T> where T : class
    {
        void Seed(EntityTypeBuilder<T> builder);


    }
}
