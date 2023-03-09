using AutoMapper;
using ToDoList.DTOs;
using ToDoList.Models;

namespace ToDoList.AutoMapper
{
    public class TaskMapper : Profile
    {
        public TaskMapper()
        {
            CreateMap<TaskToDo, TaskRequest>();
            CreateMap<TaskRequest, TaskToDo>();
        }
    }
}
