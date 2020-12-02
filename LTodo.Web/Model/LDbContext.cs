using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.Model
{
    public class LDbContext
    {
        private SqlSugarClient _dbBase;

        public ISqlSugarClient DB
        {
            get
            {
                return _dbBase;
            }
        }

        public LDbContext()
        {
            _dbBase = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=47.106.139.187;database=LTodo;uid=root;pwd=lxyLXY04/111997",
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });

            _dbBase.CodeFirst.InitTables(typeof(UserModel));
            _dbBase.CodeFirst.InitTables(typeof(TaskModel));
        }
    }
}
