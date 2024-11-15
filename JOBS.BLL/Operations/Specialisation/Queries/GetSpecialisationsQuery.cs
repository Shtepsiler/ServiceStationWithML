using AutoMapper;
using JOBS.BLL.DTOs.Respponces;
using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JOBS.BLL.Operations.Specialisation.Queries
{
    public class GetSpecialisationsQuery : IRequest<IEnumerable<SpecialisationDTO?>>
    {
    }

    public class GetSpecialisationsQueryHandler : IRequestHandler<GetSpecialisationsQuery, IEnumerable<SpecialisationDTO?>>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper _mapper;

        public GetSpecialisationsQueryHandler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SpecialisationDTO?>?> Handle(GetSpecialisationsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var specialisations = await _context.Specialisations.ToListAsync(cancellationToken);

                var dto = _mapper.Map<IEnumerable<SpecialisationDTO>>(specialisations);

                return dto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
