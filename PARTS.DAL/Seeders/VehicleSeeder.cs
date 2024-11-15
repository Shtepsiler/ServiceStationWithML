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
    public class VehicleSeeder : ISeeder<Vehicle>
    {
        public void Seed(EntityTypeBuilder<Vehicle> builder)
        {
            var vehicleFaker = new Faker<Vehicle>()
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(v => v.VIN, f => f.Vehicle.Vin())
/*                .RuleFor(v => v.Make, f => f.PickRandom<Make>())
                .RuleFor(v => v.Model, f => f.PickRandom<Model>())
                .RuleFor(v => v.SubModel, f => f.PickRandom<SubModel>())
                .RuleFor(v => v.Engine, f => f.PickRandom<Engine>())*/
                .RuleFor(v => v.URL, f => f.Internet.Url());

            builder.HasData(vehicleFaker.Generate(10));
        }
    }
}           