using LTodo.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.IRepository
{
    public interface IUserRepository
    {
        Task<bool> LoginAsync(UserModel user);
    }
}
