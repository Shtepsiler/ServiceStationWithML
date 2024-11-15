using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Interfaces;

namespace PARTS.DAL.Repositories
{
    public class MakeRepository : GenericRepository<Make>, IMakeRepository
    {
        public MakeRepository(PartsDBContext databaseContext)
            : base(databaseContext)
        {
        }
 





    }
}
