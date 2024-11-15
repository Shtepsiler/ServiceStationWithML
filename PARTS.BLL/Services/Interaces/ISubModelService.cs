using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.DAL.Entities.Vehicle;

namespace PARTS.BLL.Services.Interaces
{
    public interface ISubModelService : IGenericService<SubModel, SubModelRequest, SubModelResponse>
    {
        Task<List<ChooseSubModeResponse>> GetSubModelTitles(Guid Id);
    }
}
