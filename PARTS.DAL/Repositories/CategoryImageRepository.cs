using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Interfaces;

namespace PARTS.DAL.Repositories
{
    public class CategoryImageRepository : GenericRepository<CategoryImage>, ICategoryImageRepository
    {
        public CategoryImageRepository(PartsDBContext databaseContext)
            : base(databaseContext)
        {
        }

      




    }
}
