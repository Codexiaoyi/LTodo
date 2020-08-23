using LTodo.Web.IRepository;
using LTodo.Web.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly LDbContext _lDbContext;

        public UserRepository(LDbContext lDbContext)
        {
            _lDbContext = lDbContext;
        }

        public async Task<bool> LoginAsync(UserModel user)
        {
            var result = await _lDbContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            return result != null;
        }
    }
}
