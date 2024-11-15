using AutoMapper;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Interfaces;

namespace PARTS.BLL.Services
{
    public class EngineService : GenericService<Engine, EngineRequest, EngineResponse>, IEngineService
    {
        public EngineService(IEngineRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
