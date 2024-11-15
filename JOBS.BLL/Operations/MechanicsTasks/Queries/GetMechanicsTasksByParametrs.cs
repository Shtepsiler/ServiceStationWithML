using AutoMapper;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JOBS.BLL.Operations.MechanicsTasks.Queries
{
    public class GetMechanicsTasksByParametrs : IRequest<IEnumerable<MechanicsTasksDTO>>
    {
        public Guid JobId { get; set; }
        public Guid MechanicId { get; set; }
    }

    public class GetMechanicsTasksByParametrsHendler : IRequestHandler<GetMechanicsTasksByParametrs, IEnumerable<MechanicsTasksDTO>>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper _mapper;

        public GetMechanicsTasksByParametrsHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MechanicsTasksDTO>> Handle(GetMechanicsTasksByParametrs request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<DAL.Entities.MechanicsTasks>, IEnumerable<MechanicsTasksDTO>>(await _context.MechanicsTasks.Where(e => e.JobId == request.JobId && e.MechanicId == request.MechanicId).ToListAsync());
        }
    }


}
