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
    public class UpdateJobsMechanicCommand : IRequest
    {
        public Guid? Id { get; set; }

        public Guid? MechanicId { get; set; }

    }

    public class UpdateJobsMechanicCommandHandler : IRequestHandler<UpdateJobsMechanicCommand>
    {
        private readonly ServiceStationDBContext _context;

        public UpdateJobsMechanicCommandHandler(ServiceStationDBContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateJobsMechanicCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Jobs
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Job), request.Id);
            }
            var mech = await _context.Mechanics
    .FindAsync(new object[] { request.MechanicId }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Job), request.Id);
            }


            entity.Mechanic = mech;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
