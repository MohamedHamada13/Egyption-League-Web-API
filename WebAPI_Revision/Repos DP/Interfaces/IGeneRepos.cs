using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAPI_Revision.Entities;

namespace WebAPI_Revision.Repos_DP.Interfaces
{
    public interface IGeneRepos<T> where T : class
    {
        public Task<List<T>?> GetAllAsync(int skipedEles, int takedEles, bool isDeleted, bool isTracked); // Add IsTracked para
        public Task<T?> GetByIdAsync(int entityId, bool isTracked);
        public Task<List<T>?> FindAsync(Expression<Func<T, bool>> predicate);
        public Task<bool> IsFound(int id);
        public Task AddAsync(T entity);
        public Task AddRangeAsync(List<T> entities);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task SaveAsync();
    }
}
