using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models
{
    [Table("Task")]
    public class TaskToDo
    {
        [Key]
        public Guid TaskId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Details { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExecutionTime { get; set; }

        [Required]
        public bool IsCompleted { get; set; } = false;

        // A task belongs to a category.
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        // A task belongs to an user.
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
    }
}
