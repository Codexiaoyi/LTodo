using LTodo.Web.IRepository;
using LTodo.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.Repository
{
    public class TaskRepository : BaseRepository<TaskModel>, ITaskRepository
    {
        private readonly LDbContext dbContext;

        public TaskRepository(LDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<TaskModel>> GetAllByEmailAsync(string email)
        {
            return await dbContext.DB.Queryable<TaskModel>().Where(x => x.UserEmail == email).ToListAsync();
        }

        public async Task<int> UpdateAllAsync(List<TaskModel> tasks)
        {
            return await dbContext.DB.Updateable(tasks).ExecuteCommandAsync();
        }
    }
}
