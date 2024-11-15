using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using JOBS.DAL.Exceptions;
using MediatR;

namespace JOBS.BLL.Operations.Jobs.Commands
{
    public class DeleteJobCommand : IRequest
    {
        public Guid Id { get; set; }

        public class DeleteCategoryJobHandler : IRequestHandler<DeleteJobCommand>
        {
            private readonly ServiceStationDBContext _context;

            public DeleteCategoryJobHandler(ServiceStationDBContext context)
            {
                _context = context;
            }
            async Task IRequestHandler<DeleteJobCommand>.Handle(DeleteJobCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Jobs
                    .FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Job), request.Id);
                }

                _context.Jobs.Remove(entity);

                await _context.SaveChangesAsync(cancellationToken);

            }


        }
    }
}