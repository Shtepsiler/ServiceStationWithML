using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data;
using PARTS.DAL.Entities.Item;
using System.Text;

namespace PARTS.DAL.Seeders
{
    public class ShopSeeder
    {
        PartsDBContext dbcontext;

        public ShopSeeder(PartsDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public void Seed()
        {
            if (!dbcontext.Brands.Any())
            {
                var aiseed = new AISeeder<Brand>();
                var lsBrand = new List<Brand>();
                var titles = new HashSet<string>(); // Track unique titles
                var stringBuilder = new StringBuilder();

                stringBuilder.Append("generate json and fill them for 1 Brand, brand of auto part     public class Brand : Base\r\n    {\r\n        public string Title { get; set; }\r\n        public string? Description { get; set; } }");
                stringBuilder.Append(" except ");

                while (lsBrand.Count < 20)
                {
                    // Generate a new Brand entity synchronously (modify AISeeder if needed)
                    var brand = Task.Run(() => aiseed.GenerateEntityAsync(stringBuilder.ToString())).GetAwaiter().GetResult();
                    brand.Id = Guid.NewGuid();
                    // Ensure title uniqueness
                    if (!titles.Contains(brand.Title))
                    {
                        lsBrand.Add(brand);
                        titles.Add(brand.Title);
                        stringBuilder.Append(brand.Title + " ");
                    }
                }
                dbcontext.Brands.AddRange(lsBrand);
                dbcontext.SaveChanges();

            }
            if (!dbcontext.Categories.Any())
            {

                var aiseed = new AISeeder<Category>();
                var ls = new List<Category>();
                var titles = new HashSet<string>(); // Set to track unique titles
                var stringbuilder = new StringBuilder();
                stringbuilder.Append("generate json and fill them for 1 Category, Category of auto part public class Category : Base\r\n    {\r\n        public string Title { get; set; }\r\n        public string? Description { get; set; }\r\n}  ");
                stringbuilder.Append(" exept ");
                while (ls.Count < 20)
                {
                    try
                    {
                        var category = Task.Run(() => aiseed.GenerateEntityAsync(stringbuilder.ToString())).GetAwaiter().GetResult();
                        category.Id = Guid.NewGuid();
                        // Check if the title is unique before adding
                        if (!titles.Contains(category.Title))
                        {
                            ls.Add(category);
                            titles.Add(category.Title); // Add title to the set to track uniqueness
                            stringbuilder.Append(category.Title + " ");

                        }
                    }
                    catch { }
                }
                dbcontext.Categories.AddRange(ls);
                dbcontext.SaveChanges();
            }


            if (!dbcontext.Parts.Any())
            {
                foreach (var Category in dbcontext.Categories.ToList())
                {
                    foreach (var Brand in dbcontext.Brands.ToList())
                    {

                        var aiseed = new AISeeder<Part>();
                        var lsParts = new List<Part>();
                       // var titles = new HashSet<string>(); // Track unique titles
                        var stringBuilder = new StringBuilder();

                        stringBuilder.Append($"generate json and fill them for 1 Part from category {Category.Title} and brand {Brand.Title}, part is auto part  public class Part : Base\r\n    {{\r\n        public string? PartNumber {{ get; set; }}\r\n        public string? ManufacturerNumber {{ get; set; }}\r\n        public string? Description {{ get; set; }}\r\n        public string? PartName {{ get; set; }}\r\n        public bool? IsUniversal {{ get; set; }}\r\n        public int? PriceRegular {{ get; set; }}\r\n        public string? PartTitle {{ get; set; }}\r\n        public string? PartAttributes {{ get; set; }}\r\n        public bool? IsMadeToOrder {{ get; set; }}\r\n        public string? FitNotes {{ get; set; }}\r\n        public int? Count {{ get; set; }}\r\n     }}  ");
                        stringBuilder.Append(" except ");

                        while (lsParts.Count < 1)
                        {
                            try
                            {
                                // Generate a new Brand entity synchronously (modify AISeeder if needed)
                                var part = Task.Run(() => aiseed.GenerateEntityAsync(stringBuilder.ToString())).GetAwaiter().GetResult();
                            part.Id = Guid.NewGuid();


                            part.Category = Category;
                            part.Brand = Brand;
                                lsParts.Add(part);
                            }
                            catch { }

                        }
                        dbcontext.Parts.AddRange(lsParts);
                        dbcontext.SaveChanges();

                    }
                }
            }

        }

    }
}
