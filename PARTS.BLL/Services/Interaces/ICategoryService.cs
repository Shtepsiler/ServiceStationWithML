using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.DAL.Entities.Item;

namespace PARTS.BLL.Services.Interaces
{
    public interface ICategoryService : IGenericService<Category, CategoryRequest, CategoryResponse>
    {
    }
}
