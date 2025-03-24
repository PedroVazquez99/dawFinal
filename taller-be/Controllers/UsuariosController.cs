using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using taller_be.Models;

namespace taller_be.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly PeluqueriaBDDContext _context;
        private static List<Usuario> _usuarios = new List<Usuario>();

        public UsuariosController(PeluqueriaBDDContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        //public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        //{
        //    // Total de usuarios
        //    var totalItems = await _context.Usuarios.CountAsync();

        //    // Usuarios para la página actual
        //    var usuarios = await _context.Usuarios
        //        .OrderBy(u => u.Nombre) // Cambiar por otra columna si lo prefieres
        //        .Skip((page - 1) * pageSize) // Saltar los elementos de las páginas anteriores
        //        .Take(pageSize) // Tomar solo los elementos de la página actual
        //        .ToListAsync();

        //    // Pasar datos a la vista
        //    ViewData["TotalItems"] = totalItems; // Total de elementos
        //    ViewData["Page"] = page; // Página actual
        //    ViewData["PageSize"] = pageSize; // Tamaño de página
        //    ViewData["TotalPages"] = (int)Math.Ceiling((double)totalItems / pageSize); // Total de páginas

        //    return View(usuarios);
        //}


        // GET: Usuarios/Details/5
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

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
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

        // GET: Usuarios/Edit/5
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

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,Rol,FechaRegistro")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioExistente = await _context.Usuarios.FindAsync(id);

                    if (usuarioExistente == null)
                    {
                        return NotFound();
                    }

                    // Actualizar solo los campos modificables
                    usuarioExistente.Nombre = usuario.Nombre;
                    usuarioExistente.Email = usuario.Email;
                    usuarioExistente.Rol = usuario.Rol;
                    usuarioExistente.FechaRegistro = usuario.FechaRegistro;

                    if (!string.IsNullOrEmpty(usuario.PasswordHash))
                    {
                        var passwordHasher = new PasswordHasher<Usuario>();
                        usuarioExistente.PasswordHash = passwordHasher.HashPassword(usuarioExistente, usuario.PasswordHash);
                    }

                    _context.Update(usuarioExistente);
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


        // GET: Usuarios/Delete/5
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

        // POST: Usuarios/Delete/5
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


        private bool UsuarioExists(decimal id)
        {
            return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Método para buscar usuarios por nombre
        public async Task<IActionResult> Index(string o = "N", string d = "ASC", string searchTerm = "", int page = 1)
        {
            // Empezamos con una consulta básica de los usuarios
            IQueryable<Usuario> usuariosQuery = _context.Usuarios.AsQueryable();

            // Lógica de búsqueda (si hay término de búsqueda)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                usuariosQuery = usuariosQuery.Where(u => u.Nombre.Contains(searchTerm));
            }

            // Expresión dinámica para ordenar solo por Nombre o Email
            Expression<Func<Usuario, object>> orderByExpression = u => u.Nombre;  // Valor por defecto (Nombre)

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

            // Ordenación según dirección (ASC o DESC)
            if (d == "ASC")
            {
                usuariosQuery = usuariosQuery.OrderBy(orderByExpression);
            }
            else
            {
                usuariosQuery = usuariosQuery.OrderByDescending(orderByExpression);
            }

            // Paginación
            int pageSize = 10;  // Tamaño de página
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
            ViewData["TotalItems"] = totalItems;
            ViewData["TotalPages"] = totalPages;
            ViewData["Page"] = page;

            return View(usuariosPaged);
        }

    }
}