using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web.Model
{
    public class TaskModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public int Number { get; set; }

        public string Content { get; set; }

        public virtual UserModel User { get; set; }
    }
}
