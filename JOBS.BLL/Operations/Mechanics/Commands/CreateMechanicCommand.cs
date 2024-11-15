
using JOBS.DAL.Data;
using JOBS.DAL.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JOBS.BLL.Operations.Mechanics.Commands
{
    public class CreateMechanicCommand : IRequest<Guid>
    {
        public Guid? MechanicId { get; set; }
        public Guid? SpecialisationId { get; set; }
        public string? Specialisation { get; set; }
    }

    public class CreateMechanicCommandHandler : IRequestHandler<CreateMechanicCommand, Guid>
    {
        private readonly ServiceStationDBContext _context;

        public CreateMechanicCommandHandler(ServiceStationDBContext context)
        {
            _context = context;
        }


        public async Task<Guid> Handle(CreateMechanicCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.Mechanics.FirstOrDefault(p => p.MechanicId == request.MechanicId);
            if (entity == null) {
                entity = new Mechanic()
                {
                    MechanicId = request.MechanicId,
                    SpecialisationId = request.SpecialisationId
                };

                _context.Mechanics.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            var spec = _context.Specialisations.Where(p => p.Id == request.SpecialisationId || p.Name == request.Specialisation).FirstOrDefault();
            entity.Specialisation = spec;

            await _context.SaveChangesAsync(cancellationToken);

            return entity.MechanicId.Value;
        }

    }
}