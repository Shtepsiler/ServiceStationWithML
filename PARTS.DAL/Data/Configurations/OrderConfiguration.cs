using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Seeders;
namespace PARTS.DAL.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasMany(p=>p.Parts).WithMany(p=>p.Orders);


        }
    }
}
