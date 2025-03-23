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
    [Route("api/[controller]")]
    [ApiController]
    public class APIServiciosController : ControllerBase
    {
        private readonly PeluqueriaBDDContext _context;

        public APIServiciosController(PeluqueriaBDDContext context)
        {
            _context = context;
        }

        // GET: api/APIServicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servicio>>> GetServicio()
        {
          if (_context.Servicio == null)
          {
              return NotFound();
          }
            return await _context.Servicio.ToListAsync();
        }

        // GET: api/APIServicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servicio>> GetServicio(int id)
        {
          if (_context.Servicio == null)
          {
              return NotFound();
          }
            var servicio = await _context.Servicio.FindAsync(id);

            if (servicio == null)
            {
                return NotFound();
            }

            return servicio;
        }

        // PUT: api/APIServicios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicio(int id, Servicio servicio)
        {
            if (id != servicio.Id)
            {
                return BadRequest();
            }

            _context.Entry(servicio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioExists(id))
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

        // POST: api/APIServicios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Servicio>> PostServicio(Servicio servicio)
        {
          if (_context.Servicio == null)
          {
              return Problem("Entity set 'PeluqueriaBDDContext.Servicio'  is null.");
          }
            _context.Servicio.Add(servicio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServicio", new { id = servicio.Id }, servicio);
        }

        // DELETE: api/APIServicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            if (_context.Servicio == null)
            {
                return NotFound();
            }
            var servicio = await _context.Servicio.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }

            _context.Servicio.Remove(servicio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicioExists(int id)
        {
            return (_context.Servicio?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
