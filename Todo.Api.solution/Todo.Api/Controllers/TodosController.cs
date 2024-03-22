using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Models;

namespace Todo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoItemContext _TodoContext;
        public TodosController(TodoItemContext TodoContext)
        {
            _TodoContext=TodoContext;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TodoItem>>> GetTodos()
        {
            if (_TodoContext.Todos is null)
                return NotFound();
            return await _TodoContext.Todos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoById(int id)
        {
            if (_TodoContext.Todos is null)
                return NotFound();
            var TargetedTodo= await _TodoContext.Todos.FindAsync(id);
            if (TargetedTodo is null)
                return NotFound(id);
            return TargetedTodo;
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodo(TodoItem CreatedTodo)
        {
            _TodoContext.Todos.Add(CreatedTodo);
            await _TodoContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodoById), new { id = CreatedTodo.Id }, CreatedTodo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> UpdateTodo(int id, TodoItem UpdatedTodo)
        {
            if (id!= UpdatedTodo.Id)
                return BadRequest();

            _TodoContext.Entry(UpdatedTodo).State = EntityState.Modified;

            try
            {
                await _TodoContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoAvaliable(id))
                    return NotFound();
                else 
                    throw;
            }
            return Ok(UpdatedTodo);
        }

        private bool TodoAvaliable(int id)
        {
            return (_TodoContext.Todos?.Any(t => t.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            if (_TodoContext.Todos is null )
                return NotFound();

            var ItemToDelete = await GetTodoById(id);

            if (ItemToDelete.Value is null)
                return NotFound();

            _TodoContext.Todos.Remove(ItemToDelete.Value);

            await _TodoContext.SaveChangesAsync();

            return Ok();
        }
    }
}
