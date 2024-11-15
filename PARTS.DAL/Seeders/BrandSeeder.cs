using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
using PARTS.DAL.Entities.Item;
using System.Collections.Generic;
using System.Text;

namespace PARTS.DAL.Seeders
{
    public class BrandSeeder : ISeeder<Brand>
    {
        public void Seed(EntityTypeBuilder<Brand> builder)
        {
            var aiseed = new AISeeder<Brand>();
            var ls = new List<Brand>();
            var titles = new HashSet<string>(); // Track unique titles
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("generate json and fill them for 1 Brand, brand of auto part     public class Brand : Base\r\n    {\r\n        public string Title { get; set; }\r\n        public string? Description { get; set; } }");
            stringBuilder.Append(" except ");

            while (ls.Count < 20)
            {
                // Generate a new Brand entity synchronously (modify AISeeder if needed)
                var brand = Task.Run(() => aiseed.GenerateEntityAsync(stringBuilder.ToString())).GetAwaiter().GetResult();
                brand.Id = Guid.NewGuid();
                // Ensure title uniqueness
                if (!titles.Contains(brand.Title))
                {
                    ls.Add(brand);
                    titles.Add(brand.Title);
                    stringBuilder.Append(brand.Title + " ");
                }
            }

            builder.HasData(ls.AsReadOnly());
        }
    }
}
