//using Microsoft.Extensions.DependencyInjection;
//using PARTS.DAL.Data;
//using PARTS.DAL.Entities;
//using PARTS.DAL.Entities.Item;
//using PARTS.DAL.Entities.Vehicle;

//namespace PARTS.DAL.Seeders
//{
//    public static class Seed
//    {
//        public static async Task Initialize(IServiceProvider serviceProvider)
//        {
//            var context = serviceProvider.GetRequiredService<PartsDBContext>();

//            if (context.Vehicles.Any() || context.Parts.Any() || context.Categories.Any() || context.Brands.Any())
//            {
//                return;   // DB has been seeded
//            }

//            // Створення категорій
//            var categories = new List<Category>
//            {
//                new Category
//                {
//                    Id = Guid.NewGuid(),
//                    Title = "Engine Parts",
//                    Description = "Parts related to engine performance and maintenance",
//                    Parts = new List<Part>()
//                },
//                new Category
//                {
//                    Id = Guid.NewGuid(),
//                    Title = "Body Parts",
//                    Description = "Parts related to vehicle body and structure",
//                    Parts = new List<Part>()
//                }
//            };

//            // Створення брендів
//            var brands = new List<Brand>
//            {
//                new Brand
//                {
//                    Id = Guid.Parse("b5a0c2e2-324f-42d3-b299-28d2e12a5260"),
//                    Title = "Toyota",
//                    Description = "Toyota brand parts",
//                    Parts = new List<Part>()
//                },
//                new Brand
//                {
//                    Id = Guid.Parse("35a3c232-334f-32d3-3299-38d2e12a5260"),
//                    Title = "Honda",
//                    Description = "Honda brand parts",
//                    Parts = new List<Part>()
//                }
//            };

//            // Створення деталей
//            var parts = new List<Part>
//            {
//                new Part
//                {
//                    Id = Guid.NewGuid(),
//                    PartNumber = "ENG123",
//                    ManufacturerNumber = "MFG123",
//                    Description = "Engine Oil Filter",
//                    PartName = "Oil Filter",
//                    IsUniversal = true,
//                    PriceRegular = 25,
//                    PartTitle = "High Performance Oil Filter",
//                    PartAttributes = "Universal Fit",
//                    IsMadeToOrder = false,
//                    FitNotes = "Fits most cars",
//                    Count = 100,
//                    CategoryId = categories[0].Id,
//                    Orders = new List<Order>(),
//                    BrandId = brands[0].Id
//                },
//                new Part
//                {
//                    Id = Guid.NewGuid(),
//                    PartNumber = "BDY456",
//                    ManufacturerNumber = "MFG456",
//                    Description = "Car Door",
//                    PartName = "Front Left Door",
//                    IsUniversal = false,
//                    PriceRegular = 200,
//                    PartTitle = "Sedan Front Left Door",
//                    PartAttributes = "Color: Black",
//                    IsMadeToOrder = false,
//                    FitNotes = "Fits only sedan models",
//                    Count = 10,
//                    CategoryId = categories[1].Id,
//                    Orders = new List<Order>(),
//                    BrandId = brands[1].Id
//                }
//            };

//            // Додавання частин до категорій та брендів
//            categories[0].Parts.Add(parts[0]);
//            categories[1].Parts.Add(parts[1]);
//            brands[0].Parts.Add(parts[0]);
//            brands[1].Parts.Add(parts[1]);

//            // Створення транспортних засобів
//            var vehicles = new List<Vehicle>
//            {
//                new Vehicle
//                {
//                    Id = Guid.Parse("b5a0c2e2-3d4f-4dd3-b499-98d7e16a5360"),
//                    FullModelName = "Toyota Camry",
//                    VIN = "12345ABCDE67890",
//                    Year = new DateTime(2020, 1, 1),
//                    URL = "https://toyota.com/camry",
//                    Parts = new List<Part> { parts[0] }
//                },
//                new Vehicle
//                {
//                    Id = Guid.Parse("88c2a122-9e71-4a7a-a52d-9f82a6610d87"),
//                    FullModelName = "Honda Accord",
//                    VIN = "09876ZYXWV54321",
//                    Year = new DateTime(2019, 1, 1),
//                    URL = "https://honda.com/accord",
//                    Parts = new List<Part> { parts[1] }
//                }
//            };

//            context.Categories.AddRange(categories);
//            context.Parts.AddRange(parts);
//            context.Vehicles.AddRange(vehicles);
//            context.Brands.AddRange(brands);

//            await context.SaveChangesAsync();
//        }
//    }
//}
