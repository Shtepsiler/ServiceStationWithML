using AutoMapper;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JOBS.BLL.Operations.MechanicsTasks.Queries
{
    public class GetMechanicTaskByJobIdQuery : IRequest<IEnumerable<MechanicsTasksDTO>>
    {
        public Guid Id { get; set; }
    }
    public class GetMechanicTaskByJobIdQueryHendler : IRequestHandler<GetMechanicTaskByJobIdQuery, IEnumerable<MechanicsTasksDTO>>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper mapper;
        public GetMechanicTaskByJobIdQueryHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<MechanicsTasksDTO>> Handle(GetMechanicTaskByJobIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return mapper.Map<IEnumerable<DAL.Entities.MechanicsTasks>, IEnumerable<MechanicsTasksDTO>>(await _context.MechanicsTasks.Where(p => p.JobId == request.Id).ToListAsync());
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
