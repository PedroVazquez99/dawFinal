﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taller_be.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace taller_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIUsuariosController : ControllerBase
    {
        private readonly PeluqueriaBDDContext _context;

        public APIUsuariosController(PeluqueriaBDDContext context)
        {
            _context = context;
        }

        // GET: api/APIUsuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/APIUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(decimal id)
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/APIUsuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(decimal id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/APIUsuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'PeluqueriaBDDContext.Usuarios' is null.");
            }

            // Validar si el email ya está registrado
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
            {
                return Conflict("Ya existe un usuario registrado con este correo electrónico.");
            }

            // Generar el hash de la contraseña
            usuario.PasswordHash = HashPassword(usuario.PasswordHash);
            usuario.FechaRegistro = DateTime.UtcNow;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // Hasheo para la creacion de usuarios
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Convertir la contraseña a bytes y aplicar hash
                var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                // Convertir el hash a una cadena hexadecimal
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        // DELETE: api/APIUsuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(decimal id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(decimal id)
        {
            return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // POST: api/APIUsuarios/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (_context.Usuarios == null)
            {
                return NotFound("La base de datos de usuarios está vacía.");
            }

            // Buscar el usuario por email
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (usuario == null)
            {
                return Unauthorized("Usuario no encontrado.");
            }

            // Validar el hash de la contraseña ingresada
            if (!VerifyPassword(loginRequest.Password, usuario.PasswordHash))
            {
                return Unauthorized("Contraseña incorrecta.");
            }

            // Crear los claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("UsuarioId", usuario.Id.ToString())
            };

            // Crear un ClaimsIdentity con los claims
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Crear un ClaimsPrincipal y autenticar al usuario
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return Ok(new
            {
                Message = "Inicio de sesión exitoso.",
                Usuario = new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Email,
                    usuario.Rol,
                    usuario.FechaRegistro
                }
            });
        }

        // Método para validar el hash de la contraseña
        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                // Generar el hash de la contraseña ingresada
                var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(enteredPassword));

                // Convertir a formato hexadecimal
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                // Comparar con el hash almacenado
                return hashString == storedHash.ToLower();
            }
        }

        //Metodo para logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Account/Login");
        }

        //Metodo para recuperar el usuario
        [HttpGet("currentUser")]
        public IActionResult GetCurrentUser()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Unauthorized("El usuario no está autenticado.");
            //}

            var userIdClaim = User.FindFirst("usuarioId");
            if (userIdClaim == null)
            {
                return NotFound("El ID del usuario no está disponible.");
            }

            return Ok(new
            {
                UserId = userIdClaim.Value,
                Name = User.FindFirst(ClaimTypes.Name)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                Role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }

    }
}
