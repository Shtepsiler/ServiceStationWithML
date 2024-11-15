using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Interfaces;

namespace PARTS.DAL.Repositories
{
    public class EngineRepository : GenericRepository<Engine>, IEngineRepository
    {
        public EngineRepository(PartsDBContext databaseContext)
            : base(databaseContext)
        {
        }

      



    }
}
