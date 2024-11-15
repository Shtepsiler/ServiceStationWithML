using Bogus;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PARTS.DAL.Entities.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARTS.DAL.Seeders
{
    public class MakeSeeder : ISeeder<Make>
    {
        public void Seed(EntityTypeBuilder<Make> builder)
        {
            var makeFaker = new Faker<Make>()
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(m => m.Title, f => f.Company.CompanyName())
                .RuleFor(m => m.Description, f => f.Lorem.Sentence())
                .RuleFor(m => m.Year, f => f.Date.Past());

            builder.HasData(makeFaker.Generate(10));
        }
    }
}
