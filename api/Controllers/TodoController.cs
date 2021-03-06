#nullable disable
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Services;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoService _todoService;

        public TodoController(TodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _todoService.GetAsync();
        }

        // GET: api/Todo/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(string id)
        {
            var todoItem = await _todoService.GetAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/Todo/5
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> PutTodoItem(string id, TodoItem updatedTodo)
        {
            var todo = await _todoService.GetAsync(id);

            if (todo is null)
            {
                return NotFound();
            }

            // Input is not valid.
            if (!ValidateInput(updatedTodo))
            {
                return BadRequest();
            }

            updatedTodo.Id = todo.Id;

            await _todoService.UpdateAsync(id, updatedTodo);

            return NoContent();
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            // Input is not valid.
            if (!ValidateInput(todoItem))
            {
                return BadRequest();
            }
            
            await _todoService.CreateAsync(todoItem);

            return CreatedAtAction(nameof(GetTodoItem), new {id = todoItem.Id}, todoItem);
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(string id)
        {
            var todoItem = await _todoService.GetAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            await _todoService.RemoveAsync(id);

            return NoContent();
        }

        private bool ValidateInput(TodoItem todoItem)
        {
            // Check if the name of the item is longer than 0.
            return todoItem.Name.Length > 0;
        }
    }
}