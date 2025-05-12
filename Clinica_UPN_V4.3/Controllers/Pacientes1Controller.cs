using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinica_UPN_V4._3;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Clinica_UPN_V4._3.Controllers
{
    [Authorize]
    public class Pacientes1Controller : Controller
    {
        private readonly ClinicaUpnV4Context _context;

        public Pacientes1Controller(ClinicaUpnV4Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Salir()
        {
            await
           HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Cuenta");
        }

        // GET: Pacientes1
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pacientes.ToListAsync());
        }

        // GET: Pacientes1/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.UsuarioPac == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacientes1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Dni,Apellidos,Nombres,FechaNacimiento,Telefono,UsuarioPac,Contraseña")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(paciente);
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
                        else if (sqlEx.Message.Contains("chk_usuarioPac_length"))
                        {
                            ModelState.AddModelError("UsuarioPac", "El usuario debe tener exactamente 10 caracteres.");
                        }
                        else if (sqlEx.Message.Contains("unique_usuarioPac"))
                        {
                            ModelState.AddModelError("UsuarioPac", "El Usuario ya está registrado. Por favor, ingrese un Usuario diferente.");
                        }
                        else if (sqlEx.Message.Contains("PK__Paciente__4B504FE66A7900C0"))
                        {
                            ModelState.AddModelError("UsuarioPac", "El Usuario ya está registrado. Por favor, ingrese un Usuario diferente.");
                        }
                        else
                        {
                            _context.Add(paciente);
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
            return View(paciente);
        }

        // GET: Pacientes1/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        // POST: Pacientes1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Dni,Apellidos,Nombres,FechaNacimiento,Telefono,UsuarioPac,Contraseña")] Paciente paciente)
        {
            if (id != paciente.UsuarioPac)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.UsuarioPac))
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
            return View(paciente);
        }

        // GET: Pacientes1/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.UsuarioPac == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        /*
         public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
         */
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                try
                {
                    _context.Pacientes.Remove(paciente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlEx)
                    {
                        if (sqlEx.Message.Contains("FK__Cita__UsuarioPac__41EDCAC5"))
                        {
                            ModelState.AddModelError(string.Empty, "No se puede eliminar este paciente porque tiene citas asociadas.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ocurrió un error al eliminar el paciente. Inténtelo de nuevo más tarde.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ocurrió un error al eliminar el paciente. Inténtelo de nuevo más tarde.");
                    }

                    return View("Delete", paciente);
                }
            }

            return RedirectToAction(nameof(Index));
        }


        private bool PacienteExists(string id)
        {
            return _context.Pacientes.Any(e => e.UsuarioPac == id);
        }
    }
}
