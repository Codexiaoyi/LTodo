using System;
using System.Collections.Generic;
using System.Linq;
using LTodo.Web.IRepository;
using System.Threading.Tasks;
using LTodo.Web.Model;

namespace LTodo.Web.Repository
{
    public class UserRepository : BaseRepository<UserModel>, IUserRepository
    {
        private readonly LDbContext dbContext;

        public UserRepository(LDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// 根据邮箱查询用户
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns>用户</returns>
        public async Task<UserModel> QueryByEmailAsync(string email)
        {
            return await dbContext.DB.Queryable<UserModel>().FirstAsync(u => u.Email == email);
        }
    }
}
