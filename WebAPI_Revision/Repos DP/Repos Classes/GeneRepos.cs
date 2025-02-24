using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAPI_Revision.Data;
using WebAPI_Revision.Repos_DP.Interfaces;

namespace WebAPI_Revision.Repos_DP.Repos_Classes
{
    public class GeneRepos<T> : IGeneRepos<T> where T : class
    {
        private readonly AppDbContext _ctx;
        private readonly DbSet<T> _dbSet;

        // Ctor
        public GeneRepos(AppDbContext ctx)
        {
            _ctx = ctx;
            _dbSet = _ctx.Set<T>();
        }

        #region POST
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await SaveAsync();
        }
        #endregion

        #region GET
        public async Task<List<T>?> GetAllAsync(int skipedEles, int takedEles, bool isDeleted, bool isTracked)
        {
            List<T>? entities = (isTracked)? 
                await _dbSet
                    .Where(e => EF.Property<bool>(e, "IsDeleted") == isDeleted)
                    .Skip(skipedEles)
                    .Take(takedEles)
                    .ToListAsync()
                :
                await _dbSet
                    .AsNoTracking()
                    .Where(e => EF.Property<bool>(e, "IsDeleted") == isDeleted)
                    .Skip(skipedEles)
                    .Take(takedEles)
                    .ToListAsync();
            
            return entities;
        }

        public async Task<T?> GetByIdAsync(int entityId, bool isTracked)
        {
            // It is more recommended to use 'FindAsync()' method instead of using FirstOrDefaultAsync(), But I want you to know every thing.
            T? entity = (isTracked)?
                await _dbSet
                    .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == entityId)
                    :
                await _dbSet
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == entityId);

            return entity;
        }

        // Very important method, becouse using the delegate type
        public async Task<List<T>?> FindAsync(Expression<Func<T, bool>> predicate) // 'Expression' word to allow EFC to convert the delegate into SQL
        {
            List<T>? entities = await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();

            return entities;
        }

        public Task<bool> IsFound(int id)
        {
            return _dbSet
                .AnyAsync(e => EF.Property<int>(e, "Id") == id);
        }
        #endregion

        #region DELETE
        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }
        #endregion

        #region PUT
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await SaveAsync();
        }
        #endregion

        public async Task SaveAsync() => 
            await _ctx.SaveChangesAsync();
    }
}
