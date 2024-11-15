using AutoMapper;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Data;
using MediatR;

namespace JOBS.BLL.Operations.MechanicsTasks.Queries
{
    public class GetMechanicTaskByIdQuery : IRequest<MechanicsTasksDTO>
    {
        public Guid Id { get; set; }

    }
    public class GetMechanicTaskByIdQueryHendler : IRequestHandler<GetMechanicTaskByIdQuery, MechanicsTasksDTO>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper mapper;

        public GetMechanicTaskByIdQueryHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public async Task<MechanicsTasksDTO> Handle(GetMechanicTaskByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return mapper.Map<DAL.Entities.MechanicsTasks, MechanicsTasksDTO>(await _context.MechanicsTasks.FindAsync(request.Id, cancellationToken));
            }
            catch (Exception ex) { throw ex; }


        }
    }
}
