using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Interfaces;

namespace PARTS.DAL.Repositories
{
    public class SubModelRepository : GenericRepository<SubModel>, ISubModelRepository
    {
        public SubModelRepository(PartsDBContext databaseContext)
            : base(databaseContext)
        {
        }

       





    }
}
