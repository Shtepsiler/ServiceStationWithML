using AutoMapper;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using PARTS.DAL.Entities.Item;

namespace PARTS.BLL.Services
{
    public class BrandService : GenericService<Brand, BrandRequest, BrandResponse>, IBrandService
    {
        public BrandService(DAL.Interfaces.IBrandRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
