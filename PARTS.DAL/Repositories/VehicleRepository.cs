using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Interfaces;

namespace PARTS.DAL.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(PartsDBContext databaseContext)
            : base(databaseContext)
        {
        }
 




    }
}
