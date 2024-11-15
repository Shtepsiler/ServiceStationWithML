using JOBS.DAL.Data;
using JOBS.DAL.Exceptions;
using MediatR;

namespace JOBS.BLL.Operations.MechanicsTasks.Commands
{
    public class DeleteMechanicTaskCommand : IRequest
    {
        public Guid Id { get; set; }   
    }
    public class DeleteMechanicTaskHandler : IRequestHandler<DeleteMechanicTaskCommand>
        {
            private readonly ServiceStationDBContext _context;

            public DeleteMechanicTaskHandler(ServiceStationDBContext context)
            {
                _context = context;
            }
            async Task IRequestHandler<DeleteMechanicTaskCommand>.Handle(DeleteMechanicTaskCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.MechanicsTasks
                    .FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(DAL.Entities.MechanicsTasks), request.Id);
                }

                _context.MechanicsTasks.Remove(entity);

                await _context.SaveChangesAsync(cancellationToken);

            }


        }
}
