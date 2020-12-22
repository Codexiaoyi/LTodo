using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LToDo.Http
{

    public class BaseTaskRequest : HttpRequest
    {
        public BaseTaskRequest(string url, TaskModel task) : base(url)
        {
            Task = task;
        }

        public TaskModel Task { get; set; }
    }

    public class AddTaskRequest : BaseTaskRequest
    {
        public AddTaskRequest(TaskModel task) : base(ApiUtils.AddTask, task){}
    }

    public class UpdateTaskRequest : BaseTaskRequest
    {
        public UpdateTaskRequest(TaskModel task) : base(ApiUtils.UpdateTask, task){}
    }

    public class RemoveTasksRequest : BaseTaskRequest
    {
        public RemoveTasksRequest(TaskModel task) : base(ApiUtils.RemoveTask, task){}
    }

    public class GetAllTasksRequest : HttpRequest
    {
        public GetAllTasksRequest(string email) : base(ApiUtils.GetTask)
        {
            Email = email;
        }

        public string Email { get; set; }
    }

    public class UpdateAllTasksRequest : HttpRequest
    {
        public UpdateAllTasksRequest() : base(ApiUtils.UpdateAllTasks)
        {

        }
    }
}
