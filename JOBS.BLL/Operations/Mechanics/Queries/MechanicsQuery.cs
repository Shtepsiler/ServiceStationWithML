using AutoMapper;
using JOBS.BLL.DTOs.Requests;
using JOBS.BLL.DTOs.Respponces;
using JOBS.BLL.Operations.Jobs.Queries;
using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOBS.BLL.Operations.Mechanics.Queries
{
    public class MechanicsQuery : IRequest<IEnumerable<MechanicRequest>>
    {
    }
    public class MechanicsQueryHendler : IRequestHandler<MechanicsQuery, IEnumerable<MechanicRequest>>
    {
        private readonly ServiceStationDBContext _context;
        private readonly IMapper _mapper;

        public MechanicsQueryHendler(ServiceStationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MechanicRequest>> Handle(MechanicsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<Mechanic>, IEnumerable<MechanicRequest>>(await _context.Mechanics.ToListAsync());
        }
    }


}
