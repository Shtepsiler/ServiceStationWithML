using JOBS.DAL.Data;
using JOBS.DAL.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JOBS.BLL.Operations.MechanicsTasks.Commands;

public record UpdateMechanicTaskCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid MechanicId { get; set; }
    public Guid? JobId { get; set; }
    public string Task { get; set; }
    public string Status { get; set; }
}

public class UpdateMechanicTaskCommandHandler : IRequestHandler<UpdateMechanicTaskCommand>
{
    private readonly ServiceStationDBContext _context;

    public UpdateMechanicTaskCommandHandler(ServiceStationDBContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateMechanicTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MechanicsTasks.Include(p=>p.Mechanic).Include(p=>p.Job).AsQueryable().FirstAsync(p=>p.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(DAL.Entities.MechanicsTasks), request.Id);
        }
        var mechanic = _context.Mechanics.FirstOrDefault(p => p.MechanicId == request.MechanicId);
        entity.Mechanic = mechanic;
        var gob = _context.Jobs.FirstOrDefault(p => p.Id == request.JobId);
        entity.Job = gob;
        entity.Task = request.Task;
        entity.Status = request.Status;

        var job = await _context.Jobs
            .Include(j => j.Tasks)
            .FirstOrDefaultAsync(j => j.Id == entity.JobId, cancellationToken);

        if (job == null)
        {
            throw new NotFoundException();
        }

        // Перевіряємо статуси завдань
        if (job.Tasks.All(t => t.Status == "Completed"))
        {
            job.Status = "Completed"; // Якщо всі завдання завершені
        }
        else
        {
            job.Status = "InProgress"; // Якщо хоча б одне завдання не завершене
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
