using ToDoList.DTOs;
using ToDoList.Models;

namespace ToDoList.Services.Task
{
    public interface ITaskService
    {
        Task<Response<TaskToDo>> CreateTask(TaskRequest request, Guid userId);
        Task<List<TaskToDo>> ReadTask(Guid userId);
        Task<TaskToDo> ReadTaskById(Guid userId, Guid taskId);
        Task<Response<TaskToDo>> UpdateTask(TaskRequest request, Guid userId, Guid taskId);
        Task<List<TaskToDo>> DeleteTask(Guid userId, Guid taskId);
    }
}
