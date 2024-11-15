using JOBS.DAL.Data;
using JOBS.DAL.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOBS.BLL.Operations.Jobs.Commands
{
    public class UpdateModelApproved : IRequest
    {
        public List<Guid> ids { get; set; }

    }

    public class UpdateModelApprovedHandler : IRequestHandler<UpdateModelApproved>
    {
        private readonly ServiceStationDBContext _context;

        public UpdateModelApprovedHandler(ServiceStationDBContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateModelApproved request, CancellationToken cancellationToken)
        {
            foreach(var id in request.ids)
            {
              var entity = await _context.Jobs
                            .FindAsync(new object[] { id }, cancellationToken);
                entity.ModelAproved = true;
            }
          


            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}