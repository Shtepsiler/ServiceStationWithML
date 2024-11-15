using AutoMapper;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Interfaces;

namespace PARTS.BLL.Services
{
    public class CategoryImageService : GenericService<CategoryImage, CategoryImageRequest, CategoryImageResponse>, ICategoryImageService
    {
        public CategoryImageService(ICategoryImageRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
