using LTodo.Common;
using LTodo.Web.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskModel = LTodo.Web.Model.TaskModel;

namespace LTodo.Web.Controller
{
    [Route("api/task")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        [Authorize]
        [HttpPost("get")]
        public async Task<ActionResult> GetTask([FromBody] UserRequestDto user)
        {
            var tasks = await taskRepository.GetAllByEmailAsync(user.Email);
            return Ok(tasks);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult> AddTask([FromBody] TaskModel task)
        {
            var result = await taskRepository.AddAsync(task);
            if (result == 1)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<ActionResult> UpdateTask([FromBody] TaskModel task)
        {
            var result = await taskRepository.UpdateAsync(task);
            if (result == 1)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [Authorize]
        [HttpPost("update/all")]
        public async Task<ActionResult> UpdateAllTasks([FromBody] List<TaskModel> tasks)
        {
            var result = await taskRepository.UpdateAllAsync(tasks);
            if (result == tasks.Count)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [Authorize]
        [HttpPost("remove")]
        public async Task<ActionResult> RemoveTask([FromBody] TaskModel task)
        {
            var result = await taskRepository.DeleteByIdAsync(task.Id);
            return result ? Ok() : NotFound();
        }
    }
}
