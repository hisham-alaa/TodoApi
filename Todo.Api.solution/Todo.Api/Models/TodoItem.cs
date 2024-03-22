namespace Todo.Api.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string? Description { get; set; }
        public bool IsFinished { get; set; }
    }
}
