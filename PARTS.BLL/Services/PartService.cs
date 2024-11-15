using AutoMapper;
using Azure.Core;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Interfaces;
using System.Runtime.CompilerServices;

namespace PARTS.BLL.Services
{
    public class PartService : GenericService<Part, PartRequest, PartResponse>, IPartService
    {
        public PartService(IPartRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<IEnumerable<PartResponse>> GetPartsByOrderId(Guid orderId)
        {
            try
            {
                // Fetch parts related to the specified OrderId directly from the repository
                var entities = await _repository.GetAsync(p => p.Orders.Any(o => o.Id == orderId));

                // Map the result to PartResponse
                return _mapper.Map<IEnumerable<Part>, IEnumerable<PartResponse>>(entities);
            }
            catch (Exception)
            {
                // Preserve stack trace with "throw;" (no need to catch and rethrow without additional handling)
                throw;
            }
        }

        public override async Task<PartResponse> UpdateAsync(PartRequest request)
        {
            try
            {
                var entity = _mapper.Map<PartRequest, Part>(request);

                
                await _repository.UpdateAsync(entity);
                return _mapper.Map<Part, PartResponse>(entity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
