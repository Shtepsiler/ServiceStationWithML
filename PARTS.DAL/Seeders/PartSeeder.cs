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
    public class PartSeeder : ISeeder<Part>
    {
        public void Seed(EntityTypeBuilder<Part> builder)
        {
            var partFaker = new Faker<Part>()
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.PartNumber, f => f.Commerce.Ean13())
                .RuleFor(p => p.ManufacturerNumber, f => f.Commerce.Ean13())
                .RuleFor(p => p.Description, f => f.Lorem.Sentence())
                .RuleFor(p => p.PartName, f => f.Commerce.ProductName())
                .RuleFor(p => p.IsUniversal, f => f.Random.Bool())
                .RuleFor(p => p.PriceRegular, f => f.Random.Number(10, 100))
                .RuleFor(p => p.PartTitle, f => f.Commerce.ProductAdjective())
                .RuleFor(p => p.PartAttributes, f => f.Lorem.Sentence())
                .RuleFor(p => p.IsMadeToOrder, f => f.Random.Bool())
                .RuleFor(p => p.FitNotes, f => f.Lorem.Paragraph())
                .RuleFor(p => p.Count, f => f.Random.Number(1, 10));

            builder.HasData(partFaker.Generate(10));
        }
    }
}
