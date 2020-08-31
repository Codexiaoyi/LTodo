using LTodo.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.IRepository
{
    public interface IUserRepository
    {
        Task<UserModel> QueryByEmailAsync(string email);
        Task<int> AddAsync(UserModel user);
    }
}
