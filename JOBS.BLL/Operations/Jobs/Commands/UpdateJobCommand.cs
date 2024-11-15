using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using JOBS.DAL.Exceptions;
using MediatR;

namespace JOBS.BLL.Operations.Jobs.Commands
{
    public record UpdateJobCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid? ModelId { get; set; }
        public string? ModelName { get; set; }
        public string? Status { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? MechanicId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    }

    public class UpdateJobCommandHandler : IRequestHandler<UpdateJobCommand>
    {
        private readonly ServiceStationDBContext _context;

        public UpdateJobCommandHandler(ServiceStationDBContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateJobCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Jobs
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Job), request.Id);
            }

            entity.ManagerId = request.ManagerId;
            entity.VehicleId = request.ModelId;
            entity.ModelName = request.ModelName;
            entity.Status = request.Status;
            entity.ClientId = request.ClientId;
            entity.MechanicId = request.MechanicId;
            entity.IssueDate = request.IssueDate;
            entity.FinishDate = request.FinishDate;
            entity.Description = request.Description;
            entity.Price = request.Price;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}