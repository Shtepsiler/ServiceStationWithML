using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using JOBS.DAL.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOBS.BLL.Operations.Jobs.Commands
{
    public class UpdateJobStatusCommand : IRequest
    {
        public Guid Id { get; set; }

        public string? Status { get; set; }

    }

    public class UpdateJobStatusCommandHandler : IRequestHandler<UpdateJobStatusCommand>
    {
        private readonly ServiceStationDBContext _context;

        public UpdateJobStatusCommandHandler(ServiceStationDBContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateJobStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Jobs
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Job), request.Id);
            }
   
            entity.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
