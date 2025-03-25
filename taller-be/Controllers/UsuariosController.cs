using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using taller_be.Models;

namespace taller_be.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly PeluqueriaBDDContext _context;

        public UsuariosController(PeluqueriaBDDContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string o = "N", string d = "ASC", string searchTerm = "", string roleFilter = "", int page = 1)
        {
            IQueryable<Usuario> usuariosQuery = _context.Usuarios.AsQueryable();

            // Lógica de búsqueda 
            if (!string.IsNullOrEmpty(searchTerm))
            {
                usuariosQuery = usuariosQuery.Where(u => u.Nombre.Contains(searchTerm));
            }

            // Filtro por rol 
            if (!string.IsNullOrEmpty(roleFilter))
            {
                usuariosQuery = usuariosQuery.Where(u => u.Rol == roleFilter);
            }

            // Ordenar solo por Nombre o Email
            Expression<Func<Usuario, object>> orderByExpression = u => u.Nombre;

            switch (o)
            {
                case "N":  // Ordenar por Nombre
                    orderByExpression = u => u.Nombre;
                    break;
                case "E":  // Ordenar por Email
                    orderByExpression = u => u.Email;
                    break;
                default:
                    orderByExpression = u => u.Nombre;
                    break;
            }

            // Ordenación según dirección
            if (d == "ASC")
            {
                usuariosQuery = usuariosQuery.OrderBy(orderByExpression);
            }
            else
            {
                usuariosQuery = usuariosQuery.OrderByDescending(orderByExpression);
            }

            // Paginación
            int pageSize = 10;
            int totalItems = await usuariosQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var usuariosPaged = await usuariosQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Pasar a la vista
            ViewData["SortOrder"] = o;
            ViewData["SortDirection"] = d;
            ViewData["SearchTerm"] = searchTerm;
            ViewData["RoleFilter"] = roleFilter;
            ViewData["TotalItems"] = totalItems;
            ViewData["TotalPages"] = totalPages;
            ViewData["Page"] = page;

            return View(usuariosPaged);
        }

        // GET: Usuarios2/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios2/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Email,PasswordHash,Rol,FechaRegistro")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios2/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,PasswordHash,Rol,FechaRegistro")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios2/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'PeluqueriaBDDContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}