using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data;
using PARTS.DAL.Entities.Vehicle;
using System.Text.Json;

namespace PARTS.DAL.Seeders
{
    public class ModelSplitter
    {
        private PartsDBContext partsDBContext;

        public ModelSplitter(PartsDBContext partsDBContext)
        {
            this.partsDBContext = partsDBContext;
        }
        public bool isDataPresent()
        {
            var ifexist = partsDBContext.Models
                    .FirstOrDefault() != null ? true : false;
            return ifexist;
        }

        public (Make, Model, SubModel, Engine) SplitModelData(Class inputModel)
        {
            DateTime DateTimevalue;
            int intvalue;
            // Уникнення повторень для Make (перевіряємо чи Make вже існує)
            Make make = new Make
            {
                Title = inputModel.make_display ?? "Unknown Make",
                Description = null,
                Сountry = inputModel.make_country,
                Year = null,
                Vehicles = new List<Vehicle>(),
                Models = new List<Model>(),
                Engines = new List<Engine>()
            };

            // Створення об'єкта Model
            Model model = new Model
            {
                Title = inputModel.model_name,
                Description = inputModel.model_id,
                Seats = inputModel.model_seats,
                Year = null,
                Doors = inputModel.model_doors,
             
                Vehicles = new List<Vehicle>(),
                SubModels = new List<SubModel>()
            };

            if (DateTime.TryParse(inputModel.model_year, out DateTimevalue))
            {
                model.Year = DateTimevalue;
            }
            // Створення об'єкта SubModel
            SubModel subModel = new SubModel
            {
                Title = inputModel.model_trim ?? "Base SubModel",
                Description = inputModel.model_trim,
                Transmission = inputModel.model_transmission_type,
                Vehicles = new List<Vehicle>(),
                Engines = new List<Engine>()
            };

            if (DateTime.TryParse(inputModel.model_year, out DateTimevalue))
            {
                subModel.Year = DateTimevalue;
            }
            if (int.TryParse(inputModel.model_weight_kg, out intvalue))
            {
                subModel.Weight = intvalue;
            }
            // Створення об'єкта Engine
            Engine engine = new Engine
            {
                Fuel = inputModel.model_engine_fuel,
                Model = $"{inputModel.model_engine_position} {inputModel.model_engine_type}", // Зв'язок із Model

                Vehicles = new List<Vehicle>()
            };
            if (int.TryParse(inputModel.model_engine_cyl, out intvalue))
            {
                engine.Cylinders = intvalue;
            }
            if (int.TryParse(inputModel.model_engine_cc, out intvalue))
            {
                engine.CC = intvalue;
            }
            if (int.TryParse(inputModel.model_engine_power_hp, out intvalue))
            {
                engine.HP = intvalue;
            }


   

            return (make, model, subModel, engine);
        }

        public void Seed()
        {
            string jsonFilePath = "models.json";
            List<Class> models = ReadModelsFromJson(jsonFilePath);

            foreach (var m in models)
            {
                var (make, model, subModel, engine) = SplitModelData(m);

                // Перевіряємо існування Make
                var existingMake = partsDBContext.Makes
                    .FirstOrDefault(x => x.Title == make.Title);

                if (existingMake == null)
                {
                    partsDBContext.Makes.Add(make);
                }
                else
                {
                    make = existingMake;
                }

                // Перевіряємо існування Model
                var existingModel = partsDBContext.Models
                    .FirstOrDefault(x => x.Title == model.Title);

                if (existingModel == null)
                {
                    partsDBContext.Models.Add(model);
                    model.Make = make; // Призначаємо Make моделі
                }
                else
                {
                    model = existingModel;
                }

                // Перевіряємо існування SubModel
                var existingSubModel = partsDBContext.SubModels
                    .FirstOrDefault(x => x.Title == subModel.Title);

                if (existingSubModel == null)
                {
                    partsDBContext.SubModels.Add(subModel);
                    subModel.Model = model; // Призначаємо Model субмоделі
                }
                else
                {
                    subModel = existingSubModel;
                }

                // Перевіряємо існування Engine
                var existingEngine = partsDBContext.Engines
                    .FirstOrDefault(x => x.Cylinders == engine.Cylinders
                                      && x.CC == engine.CC
                                      && x.HP == engine.HP
                                      && x.Model == engine.Model
                                      && x.Fuel == engine.Fuel);

                if (existingEngine == null)
                {
                    partsDBContext.Engines.Add(engine);
                    engine.SubModel = subModel; // Призначаємо SubModel двигуну
                    engine.Make = make;         // Призначаємо Make двигуну
                }
                else
                {
                    engine = existingEngine;
                }

                // Пов'язуємо моделі з Make та іншими об'єктами
                if (!make.Models.Contains(model))
                {
                    make.Models.Add(model); // Додаємо модель до списку моделей Make
                }

                if (!model.SubModels.Contains(subModel))
                {
                    model.SubModels.Add(subModel); // Додаємо субмодель до списку субмоделей Model
                }

                if (!subModel.Engines.Contains(engine))
                {
                    subModel.Engines.Add(engine); // Додаємо двигун до списку двигунів SubModel
                }
                partsDBContext.SaveChanges();
            }
        }



        public void seedVehicles()
        {
            if (!partsDBContext.Vehicles.Any())
            {
                Vehicle vehicle1 = new Vehicle()
                {
                    Id = Guid.Parse("dc238098-d210-44f3-778e-08dc7b9965a3"),
                    VIN = "asd1w1vvcve1e1ew",
                    Year = DateTime.Now.AddYears(-10),
                    Timestamp = DateTime.Now,
                    Make = partsDBContext.Makes.Include(p => p.Models).ThenInclude(p => p.SubModels).ThenInclude(p => p.Engines).ElementAtOrDefault(0),
                    Model = partsDBContext.Makes.Include(p => p.Models).ThenInclude(p => p.SubModels).ThenInclude(p => p.Engines).ElementAtOrDefault(0)?.Models?.ElementAtOrDefault(0),
                    SubModel = partsDBContext.Makes.Include(p => p.Models).ThenInclude(p => p.SubModels).ThenInclude(p => p.Engines).ElementAtOrDefault(0)?.Models?.ElementAtOrDefault(0)?.SubModels?.ElementAtOrDefault(0),
                    Engine = partsDBContext.Makes.Include(p => p.Models).ThenInclude(p => p.SubModels).ThenInclude(p => p.Engines).ElementAtOrDefault(0)?.Models?.ElementAtOrDefault(0)?.SubModels?.ElementAtOrDefault(0)?.Engines?.ElementAtOrDefault(0),
                };

                Vehicle vehicle2 = new Vehicle()
                {
                    Id = Guid.Parse("dc238098-d410-44f3-778e-08dc7b9965a3"),
                    VIN = "asd1w1we1e1e1ew",
                    Year = DateTime.Now.AddYears(-10),
                    Timestamp = DateTime.Now,
                    Make = partsDBContext.Makes.Include(p=>p.Models).ThenInclude(p=>p.SubModels).ThenInclude(p=>p.Engines).ElementAtOrDefault(1),
                    Model = partsDBContext.Makes.Include(p => p.Models).ThenInclude(p => p.SubModels).ThenInclude(p => p.Engines).ElementAtOrDefault(1)?.Models?.ElementAtOrDefault(0),
                    SubModel = partsDBContext.Makes.Include(p => p.Models).ThenInclude(p => p.SubModels).ThenInclude(p => p.Engines).ElementAtOrDefault(1)?.Models?.ElementAtOrDefault(0)?.SubModels?.ElementAtOrDefault(0),
                    Engine = partsDBContext.Makes.Include(p => p.Models).ThenInclude(p => p.SubModels).ThenInclude(p => p.Engines).ElementAtOrDefault(1)?.Models?.ElementAtOrDefault(0)?.SubModels?.ElementAtOrDefault(0)?.Engines?.ElementAtOrDefault(0),
                };

                Vehicle vehicle3 = new Vehicle()
                {
                    Id = Guid.Parse("88C2A122-9E71-4A7A-A52D-9F82A6610D87"),
                    VIN = "asd1w1we1e1ennfdew",
                    Year = DateTime.Now.AddYears(-10),
                    Timestamp = DateTime.Now,
                    Make = partsDBContext.Makes.Include(p => p.Models).ThenInclude(p => p.SubModels).ThenInclude(p => p.Engines).ElementAtOrDefault(1),
                    Model = partsDBContext.Makes.Include(p => p.Models).ThenInclude(p => p.SubModels).ThenInclude(p => p.Engines).ElementAtOrDefault(1)?.Models.ElementAtOrDefault(0),
                    SubModel = partsDBContext.Makes.Include(p => p.Models).ThenInclude(p => p.SubModels).ThenInclude(p => p.Engines).ElementAtOrDefault(1)?.Models.ElementAtOrDefault(0)?.SubModels.ElementAtOrDefault(0),
                    Engine = partsDBContext.Makes.Include(p => p.Models).ThenInclude(p => p.SubModels).ThenInclude(p => p.Engines).ElementAtOrDefault(1)?.Models.ElementAtOrDefault(0)?.SubModels.ElementAtOrDefault(0)?.Engines.ElementAtOrDefault(0),
                };


                partsDBContext.Vehicles.Add(vehicle1);
                partsDBContext.Vehicles.Add(vehicle2);
                partsDBContext.Vehicles.Add(vehicle3);
                partsDBContext.SaveChanges();


            }
        }






        List<Class> ReadModelsFromJson(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Class>>(jsonString);
        }



    }
}
