using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDoList.DTOs;
using ToDoList.Models;
using ToDoList.Services.Task;

namespace ToDoList.Services
{
    public class TaskService : ITaskService
    {
        private readonly ToDoListDbContext _dbContext;
        private readonly IMapper _mapper;

        public TaskService(ToDoListDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Create task.
        public async Task<Response<TaskToDo>> CreateTask(TaskRequest request, Guid userId)
        {
            var taskMap = _mapper.Map<TaskToDo>(request);
            taskMap.UserId = userId; 

            // Add tasks.
            _dbContext.Tasks.Add(taskMap);
            await _dbContext.SaveChangesAsync();

            return new Response<TaskToDo>
            {
                Data = taskMap
            };
        }

        // Read tasks.
        public async Task<List<TaskToDo>> ReadTask(Guid userId)
        {
            var query = await _dbContext.Tasks.Where(x => x.UserId == userId).ToListAsync();

            if(query != null)
            {
                List<TaskToDo> taskToDo = _mapper.Map<List<TaskToDo>>(query);
                return taskToDo;
            }
            else
            {
                return null;
            }
        }

        // Read task by ID.
        public async Task<TaskToDo> ReadTaskById(Guid userId, Guid taskId)
        {
            var query = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.UserId == userId && x.TaskId == taskId);

            if(query != null)
            {
                TaskToDo taskById = query;
                return taskById;
            }
            else
            {
                return null;
            }
        }

        // Update task.
        public async Task<Response<TaskToDo>> UpdateTask(TaskRequest request, Guid userId, Guid taskId)
        {
            var query = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.UserId == userId && x.TaskId == taskId);

            if(query != null)
            {
                query.Title = request.Title;
                query.Details = request.Details;
                query.ExecutionTime = request.ExecutionTime;

                // Update task.
                _dbContext.Tasks.Update(query);
                await _dbContext.SaveChangesAsync();

                return new Response<TaskToDo>
                {
                    Data = query
                };
            }
            else
            {
                return null;
            }
        }

        // Delete task.
        public async Task<List<TaskToDo>> DeleteTask(Guid userId, Guid taskId)
        {
            var query = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.UserId == userId && x.TaskId == taskId);

            if(query != null)
            {
                // Delete task.
                _dbContext.Tasks.Remove(query);
                await _dbContext.SaveChangesAsync();

                // Return task list.
                var getList = await _dbContext.Tasks.Where(x => x.UserId == userId).ToListAsync();
                List<TaskToDo> taskToDos = new List<TaskToDo>(getList);
                return taskToDos;
            }
            else
            {
                return null;
            }
        }
    }
}
