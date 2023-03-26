using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoItemsController : ControllerBase
    {

        //CONSTRUCTOR
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        private bool TodoItemExists(long id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }

        [HttpGet]
        public IEnumerable<TodoItem> GetTodoItem()
        {
            return _context.TodoItems;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItemById(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // [HttpGet("{id}")]
        // public TodoItem GetTodoItemById(int id)
        // {
        //     return _context.TodoItems.FirstOrDefault(
        //         item => item.Id == id
        //     );
        // }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            //    return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

[HttpPut("{id}")]
public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
{
    if (id != todoItem.Id)
    {
        return BadRequest();
    }

    _context.Entry(todoItem).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!TodoItemExists(id))
        {
            return NotFound();
        }
        else
        {
            throw;
        }
    }

    return NoContent();
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteTodoItem(long id)
{
    var todoItem = await _context.TodoItems.FindAsync(id);
    if (todoItem == null)
    {
        return NotFound();
    }

    _context.TodoItems.Remove(todoItem);
    await _context.SaveChangesAsync();

    return NoContent();
}



        // [HttpPut("{id}")]
        //         public async Task<IActionResult> Put(int id, Evento model)
        //         {
        //             try
        //             {
        //                 var evento = await _eventoService.UpdateEvento(id, model);
        //                 if (evento == null) return BadRequest("Erro ao tentar adicionar evento.");

        //                 return Ok(evento);
        //             }
        //             catch (Exception ex)
        //             {
        //                 return this.StatusCode(StatusCodes.Status500InternalServerError,
        //                     $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
        //             }
        //         }

        //         [HttpDelete("{id}")]
        //         public async Task<IActionResult> Delete(int id)
        //         {
        //             try
        //             {
        //                 return await _eventoService.DeleteEvento(id) ? 
        //                        Ok("Deletado") : 
        //                        BadRequest("Evento não deletado");
        //             }
        //             catch (Exception ex)
        //             {
        //                 return this.StatusCode(StatusCodes.Status500InternalServerError,
        //                     $"Erro ao tentar deletar eventos. Erro: {ex.Message}");
        //             }
        //         }


    }
}
