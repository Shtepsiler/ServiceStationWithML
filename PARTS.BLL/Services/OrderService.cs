using AutoMapper;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using PARTS.DAL.Entities;
using PARTS.DAL.Interfaces;

namespace PARTS.BLL.Services
{
    public class OrderService : GenericService<Order, OrderRequest, OrderResponse>, IOrderService
    {
        IOrderRepository repository;
        public OrderService(IOrderRepository repository, IMapper mapper) : base(repository, mapper)
        {
            this.repository = repository;
        }
       

        public async Task AddPartToOrderAsync(Guid orderId, Guid partId)
        {
           await repository.AddPartToOrderAsync(orderId, partId);
        }
        public async Task RemovePartFromOrderAsync(Guid orderId, Guid partId)
        {
          await  repository.RemovePartFromOrderAsync(orderId, partId);
        }
    }
}
