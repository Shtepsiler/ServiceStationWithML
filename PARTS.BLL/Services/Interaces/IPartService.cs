using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.DAL.Entities.Item;

namespace PARTS.BLL.Services.Interaces
{
    public interface IPartService : IGenericService<Part, PartRequest, PartResponse>
    {
        Task<IEnumerable<PartResponse>> GetPartsByOrderId(Guid OrderId);
    }
}
