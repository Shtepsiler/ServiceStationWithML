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
    public class SubModelSeeder : ISeeder<SubModel>
    {
        public void Seed(EntityTypeBuilder<SubModel> builder)
        {
            var subModelFaker = new Faker<SubModel>()
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(sm => sm.Title, f => f.Vehicle.Model())
                .RuleFor(sm => sm.Description, f => f.Lorem.Sentence())
                .RuleFor(sm => sm.Year, f => f.Date.Past())
/*                .RuleFor(sm => sm.Model, f => f.PickRandom<Model>())
*/                ;

            builder.HasData(subModelFaker.Generate(10));
        }
    }
}
