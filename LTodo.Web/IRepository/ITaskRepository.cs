using LTodo.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.IRepository
{
    public interface ITaskRepository : IBaseRepository<TaskModel>
    {
        Task<List<TaskModel>> GetAllByEmailAsync(string email);
        Task<int> UpdateAllAsync(List<TaskModel> tasks);
    }
}
