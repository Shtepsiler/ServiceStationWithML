using AutoMapper;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JOBS.BLL.Operations.MechanicsTasks.Queries
{
    public class GetMechanicsTasksQuery : IRequest<IEnumerable<MechanicsTasksDTO>>
    {
    }

    public class GetMechanicsTasksQueryHendler : IRequestHandler<GetMechanicsTasksQuery, IEnumerable<MechanicsTasksDTO>>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper _mapper;

        public GetMechanicsTasksQueryHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MechanicsTasksDTO>> Handle(GetMechanicsTasksQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<DAL.Entities.MechanicsTasks>, IEnumerable<MechanicsTasksDTO>>(await _context.MechanicsTasks.ToListAsync());
        }
    }



}
