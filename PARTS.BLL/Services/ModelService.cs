using AutoMapper;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Interfaces;

namespace PARTS.BLL.Services
{
    public class ModelService : GenericService<Model, ModelRequest, ModelResponse>, IModelService
    {
        public ModelService(IModelRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public async Task<List<ChooseModelResponse>> GetModelTitles(Guid Id)
        {
            // Викликаємо асинхронний метод правильно, не використовуючи .Result
            var makes = await _repository.GetAsync();

            // Преобразуємо отримані записи у список відповідей
            var chooseMakeResponses = makes.Where(p=>p.Make.Id == Id).Select(p => new ChooseModelResponse
            {
                Id = p.Id,
                Title = p.Title
            }).ToList();

            return chooseMakeResponses;
        }
    }
}
