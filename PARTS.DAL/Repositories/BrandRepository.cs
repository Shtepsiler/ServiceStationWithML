using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Interfaces;

namespace PARTS.DAL.Repositories
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(PartsDBContext databaseContext)
            : base(databaseContext)
        {
        }


    }
}
