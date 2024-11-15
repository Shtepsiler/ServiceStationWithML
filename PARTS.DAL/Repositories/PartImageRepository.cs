using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Interfaces;

namespace PARTS.DAL.Repositories
{
    public class PartImageRepository : GenericRepository<PartImage>, IPartImageRepository
    {
        public PartImageRepository(PartsDBContext databaseContext)
            : base(databaseContext)
        {
        }

      





    }
}
