using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinica_UPN_V4._3;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Clinica_UPN_V4._3.Controllers
{
    [Authorize]
    public class AdministradorsController : Controller
    {
        private readonly ClinicaUpnV4Context _context;

        public AdministradorsController(ClinicaUpnV4Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Salir()
        {
            await
           HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Cuenta");
        }

        // GET: Administradors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Administradors.ToListAsync());
        }

        // GET: Administradors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradors
                .FirstOrDefaultAsync(m => m.UsuarioAdmin == id);
            if (administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // GET: Administradors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administradors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Dni,Apellidos,Nombres,FechaNacimiento,Telefono,UsuarioAdmin,Contraseña,CodigoSeguridad")] Administrador administrador)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(administrador);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlEx)
                    {
                        if (sqlEx.Message.Contains("unique_dni"))
                        {
                            ModelState.AddModelError("Dni", "El DNI ya está registrado. Por favor, ingrese un DNI diferente.");
                        }
                        else if (sqlEx.Message.Contains("chk_dni_length"))
                        {
                            ModelState.AddModelError("Dni", "El DNI debe tener exactamente 8 dígitos.");
                        }
                        else if (sqlEx.Message.Contains("chk_telefono_length"))
                        {
                            ModelState.AddModelError("Telefono", "El teléfono debe tener exactamente 9 dígitos.");
                        }
                        else if (sqlEx.Message.Contains("chk_usuarioAdmin_length"))
                        {
                            ModelState.AddModelError("UsuarioAdmin", "El usuario debe tener exactamente 13 caracteres.");
                        }
                        else if (sqlEx.Message.Contains("unique_usuarioAdmin"))
                        {
                            ModelState.AddModelError("UsuarioAdmin", "El Usuario ya está registrado. Por favor, ingrese un Usuario diferente.");
                        }
                        else if (sqlEx.Message.Contains("unique_codigoSeguridad"))
                        {
                            ModelState.AddModelError("CodigoSeguridad", "El código de seguridad ya está registrado. Por favor, ingrese un código de seguridad diferente.");
                        }
                        else if (sqlEx.Message.Contains("PK__Administ__5322089BD9FD7F23"))
                        {
                            ModelState.AddModelError("UsuarioAdmin", "El Usuario ya está registrado. Por favor, ingrese un Usuario diferente.");
                        }
                        else
                        {
                            _context.Add(administrador);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar los datos. Inténtelo de nuevo más tarde.");
                    }
                }
            }
            return View(administrador);
        }

        // GET: Administradors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradors.FindAsync(id);
            if (administrador == null)
            {
                return NotFound();
            }
            return View(administrador);
        }

        // POST: Administradors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Dni,Apellidos,Nombres,FechaNacimiento,Telefono,UsuarioAdmin,Contraseña,CodigoSeguridad")] Administrador administrador)
        {
            if (id != administrador.UsuarioAdmin)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(administrador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdministradorExists(administrador.UsuarioAdmin))
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
            return View(administrador);
        }

        // GET: Administradors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var administrador = await _context.Administradors
                .FirstOrDefaultAsync(m => m.UsuarioAdmin == id);
            if (administrador == null)
            {
                return NotFound();
            }

            return View(administrador);
        }

        // POST: Administradors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var administrador = await _context.Administradors.FindAsync(id);
            if (administrador != null)
            {
                _context.Administradors.Remove(administrador);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdministradorExists(string id)
        {
            return _context.Administradors.Any(e => e.UsuarioAdmin == id);
        }
    }
}
