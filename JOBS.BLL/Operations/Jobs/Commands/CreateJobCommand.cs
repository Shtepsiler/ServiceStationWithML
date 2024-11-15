using JOBS.BLL.DTOs.Requests;
using JOBS.BLL.DTOs.Respponces;
using JOBS.BLL.Helpers;
using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace JOBS.BLL.Operations.Jobs.Commands;

public record CreateJobCommand : IRequest<Guid>
{
    /*    public int? ManagerId { get; set; }*/
    public Guid? ClientId { get; set; }
    public Guid? VehicleId { get; set; }
    public DateTime IssueDate { get; set; }
    public string Description { get; set; }
}

public class CreateJobHandler : IRequestHandler<CreateJobCommand, Guid>
{
    private readonly ServiceStationDBContext _context;

    public CreateJobHandler(ServiceStationDBContext context,IHttpClientFactory httpClientFactory)
    {
        _context = context;
        HttpClientFactory = httpClientFactory;
    }

    public IHttpClientFactory HttpClientFactory { get; }

    public async Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var entity = new Job()
        {
            /* ManagerId = request.ManagerId,*/
            VehicleId = request.VehicleId,
            ClientId = request.ClientId,
            IssueDate = request.IssueDate,
            Description = request.Description
        };

        ApiHttpClient apiCliet = new(HttpClientFactory.CreateClient("Model"));
        var modelreq = new ModelPredictReqest();
        modelreq.description = entity.Description;
        ModelPredictResoponce modelPredictResoponce = null;
        try
        {
            modelPredictResoponce = await apiCliet.PostAsync<ModelPredictReqest, ModelPredictResoponce>("predict", modelreq);
        }
        catch(Exception e)
        {
            throw;
        }

        if (modelPredictResoponce == null)
            throw new Exception("модель не повернула результат");


        var spec = _context.Specialisations.Where(p=>p.Name == modelPredictResoponce.predicted_class).FirstOrDefault();

        var mechanic = MechanicScheduler.AssignTaskToLeastBusyMechanic(_context, spec, entity.IssueDate, TimeSpan.FromHours(1));
        entity.Status = "NewJob";
        entity.Mechanic = mechanic;
        entity.ModelConfidence = modelPredictResoponce.confidence;
        entity.ModelAproved = modelPredictResoponce.confidence > 0.7;
        await _context.Jobs.AddAsync(entity);


        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

}
