using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.DAL.Entities;

namespace PARTS.BLL.Services.Interaces
{
    public interface IOrderService : IGenericService<Order, OrderRequest, OrderResponse>
    {
       Task AddPartToOrderAsync( Guid orderId, Guid partId);
        Task RemovePartFromOrderAsync(Guid orderId, Guid partId);
    }
}
