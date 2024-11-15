using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.DAL.Entities.Vehicle;

namespace PARTS.BLL.Services.Interaces
{
    public interface IMakeService : IGenericService<Make, MakeRequest, MakeResponse>
    {
        Task<List<ChooseMakeResponse>> GetMakeTitles();
    }
}
