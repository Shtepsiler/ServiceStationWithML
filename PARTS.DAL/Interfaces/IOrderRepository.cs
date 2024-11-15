using PARTS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARTS.DAL.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task AddPartToOrderAsync(Guid orderId, Guid partId);
        Task RemovePartFromOrderAsync(Guid orderId, Guid partId);
    }
}
