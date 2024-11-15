using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.DAL.Entities.Item;

namespace PARTS.BLL.Services.Interaces
{
    public interface IPartImageService : IGenericService<PartImage, PartImageRequest, PartImageResponse>
    {
    }
}
