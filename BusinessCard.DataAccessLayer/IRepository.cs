using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<int> AddAsync(TEntity newData);

        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetAsync(int id);

        Task UpdateAsync(int id, TEntity updateData);

        Task<bool> DeleteAsync(int id);
    }
}