using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LToDo.Database
{
    public class SqliteManager
    {
        public SQLiteConnection db;

        private static readonly Lazy<SqliteManager> _sqliteHelper = new Lazy<SqliteManager>(() => new SqliteManager());

        public static SqliteManager Instance
        {
            get
            {
                return _sqliteHelper.Value;
            }
        }

        private SqliteManager()
        {
            var baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetExecutingAssembly().GetName().Name);
            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }
            db = new SQLiteConnection(Path.Combine(baseFolder, "LTodo.db"));
            db.CreateTable<TaskModel>();
        }

        public int Add<T>(T model)
        {
            return db.Insert(model);
        }

        public int Update<T>(T model)
        {
            return db.Update(model);
        }

        public int Delete<T>(T model)
        {
            return db.Delete(model);
        }

        public List<T> Query<T>(string sql) where T : new()
        {
            return db.Query<T>(sql);
        }

        public void UpdateTransaction<T>(T[] models)
        {
            db.BeginTransaction();
            foreach (var model in models)
            {
                db.Update(model);
            }
            db.Commit();
        }

        public void DeleteDatabase()
        {
            db.DropTable<TaskModel>();
            db.CreateTable<TaskModel>();
        }
    }
}
