using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.Model
{
    public class TaskModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }

        public bool IsEnabled { get; set; }

        public int Number { get; set; }

        public string Content { get; set; }

        public string UserEmail { get; set; }
    }
}
