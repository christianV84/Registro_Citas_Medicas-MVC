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
    public class MedicosController : Controller
    {
        private readonly ClinicaUpnV4Context _context;

        public MedicosController(ClinicaUpnV4Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Salir()
        {
            await
           HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Cuenta");
        }

        // GET: Medicos/Index
        public async Task<IActionResult> Index(string searchString)
        {

            var medicos = await Medico.BuscarMedicosAsync(_context, searchString);

            var nombresCompletos = medicos.Select(m => m.NombreCompletoMedico).ToList();

            return View(medicos);
        }
        
        // GET: Medicos
        /*
        public async Task<IActionResult> Index()
        {
            return View(await _context.Medicos.ToListAsync());
        }
        */

        // GET: Medicos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .FirstOrDefaultAsync(m => m.UsuarioMed == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // GET: Medicos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Dni,Apellidos,Nombres,FechaNacimiento,Telefono,NumColegiatura,Especializacion,UsuarioMed,Contraseña")] Medico medico)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(medico);
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
                        else if (sqlEx.Message.Contains("chk_usuarioMed_length"))
                        {
                            ModelState.AddModelError("UsuarioMed", "El usuario debe tener exactamente 12 caracteres.");
                        }
                        else if (sqlEx.Message.Contains("PK__Medico__7F455BA0C12668AA"))
                        {
                            ModelState.AddModelError("UsuarioMed", "El Usuario ya está registrado. Por favor, ingrese un Usuario diferente.");
                        }
                        else if (sqlEx.Message.Contains("unique_numColegiatura"))
                        {
                            ModelState.AddModelError("NumColegiatura", "El número de colegiatura ya está registrado. Por favor, ingrese un número de colegiatura diferente.");
                        }
                        else
                        {
                            _context.Add(medico);
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
            return View(medico);
        }

        // GET: Medicos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            return View(medico);
        }

        // POST: Medicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Dni,Apellidos,Nombres,FechaNacimiento,Telefono,NumColegiatura,Especializacion,UsuarioMed,Contraseña")] Medico medico)
        {
            if (id != medico.UsuarioMed)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicoExists(medico.UsuarioMed))
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
            return View(medico);
        }

        // GET: Medicos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .FirstOrDefaultAsync(m => m.UsuarioMed == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        /*
          public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico != null)
            {
                _context.Medicos.Remove(medico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
         */
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico != null)
            {
                try
                {
                    _context.Medicos.Remove(medico);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlEx)
                    {
                        if (sqlEx.Message.Contains("FK__Cita__UsuarioMed__40F9A68C"))
                        {
                            ModelState.AddModelError(string.Empty, "No se puede eliminar este médico porque tiene citas asociadas.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ocurrió un error al eliminar el medico. Inténtelo de nuevo más tarde.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ocurrió un error al eliminar el paciente. Inténtelo de nuevo más tarde.");
                    }

                    return View("Delete", medico);
                }
            }

            return RedirectToAction(nameof(Index));
        }


        private bool MedicoExists(string id)
        {
            return _context.Medicos.Any(e => e.UsuarioMed == id);
        }
    }
}
