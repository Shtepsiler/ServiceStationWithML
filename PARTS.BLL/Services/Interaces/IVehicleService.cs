using Azure.Core;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.DAL.Entities.Vehicle;

namespace PARTS.BLL.Services.Interaces
{
    public interface IVehicleService : IGenericService<Vehicle, VehicleRequest, VehicleResponse>
    {
        Task<Guid> CreateVehicle(CreateVehicleRequest request);
        public Task<VehicleResponse> PostAsync(VehicleRequest request);
    }
}
