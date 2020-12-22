using LTodo.Common;
using LTodo.Web.Dto;
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
    [Authorize]
    [Route("api/task")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        [HttpPost("get")]
        public async Task<ActionResult> GetTasks([FromBody] UserRequestDto user)
        {
            var tasks = await taskRepository.GetAllByEmailAsync(user.Email);
            return Ok(tasks);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddTask([FromBody] TaskRequestDto task)
        {
            var result = await taskRepository.AddAsync(task.Task);
            if (result == 1)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateTask([FromBody] TaskRequestDto task)
        {
            var result = await taskRepository.UpdateAsync(task.Task);
            if (result == 1)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost("update/all")]
        public async Task<ActionResult> UpdateAllTasks([FromBody] AllTaskRequestDto tasks)
        {
            var result = await taskRepository.UpdateAllAsync(tasks.Tasks);
            if (result == tasks.Tasks.Count)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost("remove")]
        public async Task<ActionResult> RemoveTask([FromBody] TaskRequestDto task)
        {
            var result = await taskRepository.DeleteByIdAsync(task.Task.Id);
            return result ? Ok() : NotFound();
        }
    }
}
