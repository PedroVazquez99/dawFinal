using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller_be.Models;

namespace taller_be.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class ListasController : ControllerBase
    //{
    //    private readonly DBTallerVUE _context;

    //    public ListasController(DBTallerVUE context)
    //    {
    //        _context = context;
    //    }

    //    // GET: api/Listas
    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<TaskList>>> GetTaskLists()
    //    {
    //      if (_context.TaskLists == null)
    //      {
    //          return NotFound();
    //      }
    //        return await _context.TaskLists.ToListAsync();
    //    }

    //    // GET: api/Listas/5
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<TaskList>> GetTaskList(decimal id)
    //    {
    //      if (_context.TaskLists == null)
    //      {
    //          return NotFound();
    //      }
    //        var taskList = await _context.TaskLists.FindAsync(id);

    //        if (taskList == null)
    //        {
    //            return NotFound();
    //        }

    //        return taskList;
    //    }

    //    // PUT: api/Listas/5
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> PutTaskList(decimal id, TaskList taskList)
    //    {
    //        if (id != taskList.Id)
    //        {
    //            return BadRequest();
    //        }

    //        _context.Entry(taskList).State = EntityState.Modified;

    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!TaskListExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return NoContent();
    //    }

    //    // POST: api/Listas
    //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPost]
    //    public async Task<ActionResult<TaskList>> PostTaskList(TaskList taskList)
    //    {
    //      if (_context.TaskLists == null)
    //      {
    //          return Problem("Entity set 'DBTallerVUE.TaskLists'  is null.");
    //      }
    //        _context.TaskLists.Add(taskList);
    //        await _context.SaveChangesAsync();

    //        return CreatedAtAction("GetTaskList", new { id = taskList.Id }, taskList);
    //    }

    //    // DELETE: api/Listas/5
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteTaskList(decimal id)
    //    {
    //        if (_context.TaskLists == null)
    //        {
    //            return NotFound();
    //        }
    //        var taskList = await _context.TaskLists.FindAsync(id);
    //        if (taskList == null)
    //        {
    //            return NotFound();
    //        }

    //        _context.TaskLists.Remove(taskList);
    //        await _context.SaveChangesAsync();

    //        return NoContent();
    //    }

    //    private bool TaskListExists(decimal id)
    //    {
    //        return (_context.TaskLists?.Any(e => e.Id == id)).GetValueOrDefault();
    //    }
    //}
}
