using AutoMapper;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Interfaces;

namespace PARTS.BLL.Services
{
    public class SubModelService : GenericService<SubModel, SubModelRequest, SubModelResponse>, ISubModelService
    {
        public SubModelService(ISubModelRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public async Task<List<ChooseSubModeResponse>> GetSubModelTitles(Guid Id)
        {
            // Викликаємо асинхронний метод правильно, не використовуючи .Result
            var makes = await _repository.GetAsync();

            // Преобразуємо отримані записи у список відповідей
            var chooseMakeResponses = makes.Where(p=>p.Model.Id == Id).Select(p => new ChooseSubModeResponse
            {
                Id = p.Id,
                Title = p.Title
            }).ToList();

            return chooseMakeResponses;
        }
    }
}
