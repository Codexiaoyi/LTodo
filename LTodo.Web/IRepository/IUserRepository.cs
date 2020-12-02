using LTodo.Web.Model;
using LTodo.Web.Repository;
using System.Threading.Tasks;

namespace LTodo.Web.IRepository
{
    public interface IUserRepository : IBaseRepository<UserModel>
    {
        Task<UserModel> QueryByEmailAsync(string id);
    }
}
