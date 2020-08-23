using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.Model
{
    public class LDbContext : DbContext
    {
        public LDbContext()
        {
        }

        public LDbContext(DbContextOptions<LDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=47.106.139.187;database=LTodo;uid=root;pwd=lxyLXY04/111997");
            }
        }

        public virtual DbSet<TaskModel> Tasks { get; set; }
        public virtual DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
