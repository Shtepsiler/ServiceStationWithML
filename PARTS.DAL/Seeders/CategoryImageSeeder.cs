using Bogus;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARTS.DAL.Seeders
{
    public class CategoryImageSeeder : ISeeder<CategoryImage>
    {
        public void Seed(EntityTypeBuilder<CategoryImage> builder)
        {
            var categoryImageFaker = new Faker<CategoryImage>()
                .RuleFor(p => p.Id, f => f.Random.Guid())

                .RuleFor(ci => ci.SourceContentType, f => f.Lorem.Word())
                .RuleFor(ci => ci.HashFileContent, f => f.Random.Guid().ToString())
                .RuleFor(ci => ci.Size, f => f.Random.Number(1000, 5000))
                .RuleFor(ci => ci.Path, f => f.Lorem.Word())
                .RuleFor(ci => ci.Title, f => f.Lorem.Word());

            builder.HasData(categoryImageFaker.Generate(10));
        }
    }
}
