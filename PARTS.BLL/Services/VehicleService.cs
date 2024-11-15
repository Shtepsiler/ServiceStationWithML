using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using PARTS.DAL.Data;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Interfaces;

namespace PARTS.BLL.Services
{
    public class VehicleService : GenericService<Vehicle, VehicleRequest, VehicleResponse>, IVehicleService
    {
        public PartsDBContext DBContext { get; }

        public VehicleService(PartsDBContext dBContext,IVehicleRepository repository, IMapper mapper) : base(repository, mapper)
        {
            DBContext = dBContext;
        }

        public async Task<string> GetModelNameById(Guid id)
        {

            return _repository.GetByIdAsync(id).Result.FullModelName;

        }
        public async Task<Guid> CreateVehicle(CreateVehicleRequest request)
        {
            // Створюємо новий екземпляр Vehicle
            Vehicle vehicle = new();

            // Перевіряємо, чи вказано коректне значення для року
            if (request.Year > 0)
            {
                string data = $"{request.Year}/01/01";


                if (DateTime.TryParse(data, out DateTime date))
                    vehicle.Year = date;
                else vehicle.Year = DateTime.Parse("2000/01/01");
            }
            else
            {
                throw new ArgumentException("Invalid year value.");
            }

            // Перевірка VIN
            if (string.IsNullOrEmpty(request.VIN))
            {
                throw new ArgumentException("VIN cannot be empty.");
            }
            vehicle.VIN = request.VIN;

            // Перевіряємо наявність записів для Make, Model та SubModel
            var make = DBContext.Makes.FirstOrDefault(p => p.Id == request.MakeId);
            var model = DBContext.Models.FirstOrDefault(p => p.Id == request.ModelId);
            var submodel = DBContext.SubModels.FirstOrDefault(p => p.Id == request.SubModelId);

            // Якщо make не знайдений, кидаємо помилку
            if (make == null)
            {
                throw new ArgumentException("Make not found.");
            }
            vehicle.Make = make; // Присвоюємо значення Make

            // Якщо model не знайдений, кидаємо помилку
            if (model == null)
            {
                throw new ArgumentException("Model not found.");
            }
            vehicle.Model = model; // Присвоюємо значення Model

            // Якщо submodel не знайдений, кидаємо помилку
            if (submodel == null)
            {
                throw new ArgumentException("SubModel not found.");
            }
            vehicle.SubModel = submodel; // Присвоюємо значення SubModel

            // Зберігаємо новий Vehicle в базу даних
            await _repository.InsertAsync(vehicle);


            // Повертаємо ID новоствореного Vehicle
            return vehicle.Id;
        }




        public async Task<VehicleResponse> PostAsync(VehicleRequest request)
        {
            try
            {
                var entity = _mapper.Map<VehicleRequest, Vehicle>(request);
                await _repository.InsertAsync(entity);
                return _mapper.Map<Vehicle, VehicleResponse>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
