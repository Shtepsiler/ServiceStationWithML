using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.DAL.Entities.Vehicle;

namespace PARTS.BLL.Services.Interaces
{
    public interface IModelService : IGenericService<Model, ModelRequest, ModelResponse>
    {
        Task<List<ChooseModelResponse>> GetModelTitles(Guid Id);
    }
}
