using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Name { get; set; }

        [Required, EmailAddress]
        [ConcurrencyCheck]
        public string Email { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Password { get; set; }

        // A user contains many tasks.
        public virtual ICollection<TaskToDo> Tasks { get; set; }
    }
}
