using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.IRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity> QueryByIdAsync(string id);
        Task<int> AddAsync(TEntity entity);
        Task<bool> DeleteByIdAsync(string id);
        Task<int> UpdateAsync(TEntity entity);
    }
}
