using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
    public class ToDoListDbContext : DbContext
    {
        // Constructor declaration.
        public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskToDo> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed categories.
            modelBuilder.Entity<Category>().HasData(
                new { CategoryId = 1, CategoryName = "Work" },
                new { CategoryId = 2, CategoryName = "Fun" },
                new { CategoryId = 3, CategoryName = "Sport" },
                new { CategoryId = 4, CategoryName = "Study" },
                new { CategoryId = 5, CategoryName = "Family" },
                new { CategoryId = 6, CategoryName = "Birth" });

            base.OnModelCreating(modelBuilder);
        } 
    }
}
