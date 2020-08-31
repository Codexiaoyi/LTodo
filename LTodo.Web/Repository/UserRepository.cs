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

        public async Task<int> AddAsync(UserModel user)
        {
            await _lDbContext.Users.AddAsync(user);
            return await _lDbContext.SaveChangesAsync();
        }

        public async Task<UserModel> QueryByEmailAsync(string email)
        {
            return await _lDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
