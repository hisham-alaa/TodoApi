using Microsoft.EntityFrameworkCore;

namespace Todo.Api.Models
{
    public class TodoItemContext : DbContext
    {
        public TodoItemContext(DbContextOptions<TodoItemContext> options) : base(options) 
        { }

        public DbSet<TodoItem> Todos { get; set; }

    }
}
