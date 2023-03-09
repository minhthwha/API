using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.DTOs;
using ToDoList.Models;
using ToDoList.Services.Task;

namespace ToDoList.Controllers
{
    [Authorize] // Authorize attribute => All of the APIs in controller are secured by token.
    [Route("api/Tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // Create task.
        [HttpPost("Create")]
        public async Task<ActionResult<Response<TaskRequest>>> CreateTask(TaskRequest taskCreate, Guid userId)
        {
            // Verify user.
            userId = Guid.Parse(HttpContext.User.Claims.Where(t => t.Type == ClaimTypes.NameIdentifier).Select(t => t.Value).FirstOrDefault());

            // Get data from TaskService.
            var taskCreation = await _taskService.CreateTask(taskCreate, userId);

            return Ok(new Response<TaskRequest>
            {
                Data = taskCreate
            });
        }

        // Read task.
        [HttpGet("Read")]
        public async Task<ActionResult<TaskToDo>> ReadTask(Guid userId)
        {
            // Verify user.
            userId = Guid.Parse(HttpContext.User.Claims.Where(t => t.Type == ClaimTypes.NameIdentifier).Select(t => t.Value).FirstOrDefault());

            // Get data from TaskService.
            var taskReading = await _taskService.ReadTask(userId);

            return Ok(taskReading);
        }

        // Read task by ID.
        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskToDo>> ReadTaskById(Guid userId, Guid taskId)
        {
            // Verify user.
            userId = Guid.Parse(HttpContext.User.Claims.Where(t => t.Type == ClaimTypes.NameIdentifier).Select(t => t.Value).FirstOrDefault());

            // Get data from TaskService.
            var taskReadById = await _taskService.ReadTaskById(userId, taskId);

            return Ok(taskReadById);
        }

        // Update task.
        [HttpPut("Update")]
        public async Task<ActionResult<Response<TaskRequest>>> UpdateTask(TaskRequest taskUpdate, Guid userId, Guid taskId)
        {
            // Verify user.
            userId = Guid.Parse(HttpContext.User.Claims.Where(t => t.Type == ClaimTypes.NameIdentifier).Select(t => t.Value).FirstOrDefault());

            // Get data from TaskService.
            var taskUpdating = await _taskService.UpdateTask(taskUpdate, userId, taskId);

            return Ok(new Response<TaskRequest>
            {
                Data = taskUpdate
            });
        }

        // Delete task.
        [HttpDelete("Delete")]
        public async Task<ActionResult<TaskToDo>> DeleteTask(Guid userId, Guid taskId)
        {
            // Verify user.
            userId = Guid.Parse(HttpContext.User.Claims.Where(t => t.Type == ClaimTypes.NameIdentifier).Select(t => t.Value).FirstOrDefault());

            // Get data from TaskService.
            var taskDeleting = await _taskService.DeleteTask(userId, taskId);

            return Ok(taskDeleting);
        }
    }
}
