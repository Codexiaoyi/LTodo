using LTodo.Web.IRepository;
using LTodo.Web.Model;
using System.Threading.Tasks;

namespace LTodo.Web.Repository
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly LDbContext dbContext;

        public BaseRepository(LDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id">id(主键)</param>
        /// <returns>实体</returns>
        public async Task<TEntity> QueryByIdAsync(string id)
        {
            return await dbContext.DB.Queryable<TEntity>().In<string>(id).SingleAsync();
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public async Task<int> AddAsync(TEntity entity)
        {
            return await dbContext.DB.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id">id(主键)</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteByIdAsync(string id)
        {
            return await dbContext.DB.Deleteable<TEntity>().In(id).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响行数</returns>
        public async Task<int> UpdateAsync(TEntity entity)
        {
            return await dbContext.DB.Updateable(entity).ExecuteCommandAsync();
        }
    }
}
