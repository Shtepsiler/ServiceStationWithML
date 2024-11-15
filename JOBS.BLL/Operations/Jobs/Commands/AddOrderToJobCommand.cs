using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using MediatR;

namespace JOBS.BLL.Operations.Jobs.Commands;

public record AddOrderToJobCommand : IRequest<Guid>
{
    public Guid? Id { get; set; }
    public Guid? OrderId { get; set; }

}

public class AddOrderToJobCommandHandler : IRequestHandler<AddOrderToJobCommand, Guid>
{
    private readonly ServiceStationDBContext _context;

    public AddOrderToJobCommandHandler(ServiceStationDBContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(AddOrderToJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _context.Jobs.FindAsync(new object[] { request.Id }, cancellationToken);
        job.OrderId
             = request.OrderId;
        await _context.SaveChangesAsync();
 
        return job.Id;
    }

}
