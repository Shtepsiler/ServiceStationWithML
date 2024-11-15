using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data;
using PARTS.DAL.Excepstions;
using PARTS.DAL.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace PARTS.DAL.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly PartsDBContext databaseContext;
        protected readonly DbSet<TEntity> table;

        public GenericRepository(PartsDBContext databaseContext)
        {
            this.databaseContext = databaseContext;
            table = this.databaseContext.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity?>?> GetAsync()
        {
            var query = IncludeNavigationProperties(table);
            var entities = await query.ToListAsync();
            if (entities == null || !entities.Any())
            {
                throw new EntityNotFoundException("No entities found in this table.");
            }
            return entities;
        }
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var query = IncludeNavigationProperties(table);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            var query = IncludeNavigationProperties(table);
            var entity = await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
            if (entity == null)
            {
                throw new EntityNotFoundException(GetEntityNotFoundErrorMessage(id));
            }
            return entity;
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            await table.AddAsync(entity);
            await databaseContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            table.Update(entity);
            await databaseContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await table.FindAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(GetEntityNotFoundErrorMessage(id));
            }
            table.Remove(entity);
            await databaseContext.SaveChangesAsync();
        }

        private IQueryable<TEntity> IncludeNavigationProperties(IQueryable<TEntity> query)
        {
            // Динамічно додає всі навігаційні властивості
            var entityType = databaseContext.Model.FindEntityType(typeof(TEntity));
            var navigationProperties = entityType?.GetNavigations().Select(n => n.Name) ?? Enumerable.Empty<string>();

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }

            return query;
        }


        protected static string GetEntityNotFoundErrorMessage(Guid id) =>
            $"{typeof(TEntity).Name} with ID {id} not found.";
    }
}
