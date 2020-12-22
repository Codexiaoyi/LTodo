using LTodo.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTodo.Web.Dto
{
    public class TaskRequestDto
    {
        public TaskModel Task { get; set; }
    }

    public class AllTaskRequestDto
    {
        public List<TaskModel> Tasks { get; set; }
    }
}
