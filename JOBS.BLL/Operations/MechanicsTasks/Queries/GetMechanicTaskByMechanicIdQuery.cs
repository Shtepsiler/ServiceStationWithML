using AutoMapper;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JOBS.BLL.Operations.MechanicsTasks.Queries
{
    public class GetMechanicTaskByMechanicIdQuery : IRequest<IEnumerable<MechanicsTasksDTO>>
    {
        public Guid Id { get; set; }

    }
    public class GetMechanicTaskByMechanicIdQueryHendler : IRequestHandler<GetMechanicTaskByMechanicIdQuery, IEnumerable<MechanicsTasksDTO>>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper mapper;

        public GetMechanicTaskByMechanicIdQueryHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MechanicsTasksDTO>> Handle(GetMechanicTaskByMechanicIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var t = _context.MechanicsTasks.Select(p => p.Id).ToList();


                var tasks = await _context.MechanicsTasks.Where(p => p.MechanicId == request.Id).ToListAsync();
                return mapper.Map<IEnumerable<DAL.Entities.MechanicsTasks>, IEnumerable<MechanicsTasksDTO>>(tasks);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
