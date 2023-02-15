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
        [ConcurrencyCheck] // Khi sửa/xóa thì kiểm tra xem giá trị đã thay đổi hay chưa.
        public string Name { get; set; }
        [Required, EmailAddress]
        [ConcurrencyCheck]
        public string Email { get; set; }
        [Required]
        [ConcurrencyCheck]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        [Required]
        //[MinLength(6)]
        public string UserName { get; set; }
        [Required]
        //[Range(8, 16)]
        [ConcurrencyCheck]
        public string Password { get; set; }
    }
}
