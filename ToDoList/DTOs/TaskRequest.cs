namespace ToDoList.DTOs
{
    public class TaskRequest
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public int CategoryId { get; set; }
        public DateTime ExecutionTime { get; set; }
    }
}
