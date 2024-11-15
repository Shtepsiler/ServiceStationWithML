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
    public class EngineSeeder : ISeeder<Engine>
    {
        public void Seed(EntityTypeBuilder<Engine> builder)
        {
            var engineFaker = new Faker<Engine>()
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(e => e.Cylinders, f => f.Random.Number(4, 8))
                .RuleFor(e => e.CC, f => f.Random.Number(1, 5))
                .RuleFor(e => e.Year, f => f.Date.Past())
                .RuleFor(e => e.Model, f => f.Vehicle.Model())
/*                .RuleFor(e => e.SubModel, f => f.PickRandom<SubModel>())
                .RuleFor(e => e.Make, f => f.PickRandom<Make>()) */
                ;

            builder.HasData(engineFaker.Generate(10));
        }
    }
}
