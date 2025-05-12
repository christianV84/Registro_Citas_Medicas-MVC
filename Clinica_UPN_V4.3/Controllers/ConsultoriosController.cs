using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinica_UPN_V4._3;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Clinica_UPN_V4._3.Models;

namespace Clinica_UPN_V4._3.Controllers
{
    [Authorize]
    public class ConsultoriosController : Controller
    {
        private readonly ClinicaUpnV4Context _context;

        public ConsultoriosController(ClinicaUpnV4Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Salir()
        {
            await
           HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Cuenta");
        }

        // GET: Consultorios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Consultorios.ToListAsync());
        }

        // GET: Consultorios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultorio = await _context.Consultorios
                .FirstOrDefaultAsync(m => m.NumConsultorio == id);
            if (consultorio == null)
            {
                return NotFound();
            }

            return View(consultorio);
        }

        // GET: Consultorios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consultorios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumConsultorio,EspConsultorio")] Consultorio consultorio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(consultorio);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlEx)
                    {
                        if (sqlEx.Message.Contains("PK__Consulto__076B7593EDACCA5F"))
                        {
                            ModelState.AddModelError("NumConsultorio", "El número de consultorio debe ser único y no repertise. ");
                        }
                        else
                        {
                            _context.Add(consultorio);
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
            return View(consultorio);
        }

        // GET: Consultorios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultorio = await _context.Consultorios.FindAsync(id);
            if (consultorio == null)
            {
                return NotFound();
            }
            return View(consultorio);
        }

        // POST: Consultorios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumConsultorio,EspConsultorio")] Consultorio consultorio)
        {
            if (id != consultorio.NumConsultorio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consultorio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultorioExists(consultorio.NumConsultorio))
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
            return View(consultorio);
        }

        // GET: Consultorios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultorio = await _context.Consultorios
                .FirstOrDefaultAsync(m => m.NumConsultorio == id);
            if (consultorio == null)
            {
                return NotFound();
            }

            return View(consultorio);
        }

        // POST: Consultorios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        /*
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consultorio = await _context.Consultorios.FindAsync(id);
            if (consultorio != null)
            {
                _context.Consultorios.Remove(consultorio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (int.TryParse(id, out int consultorioId))
            {
                var consultorio = await _context.Consultorios.FindAsync(consultorioId);
                if (consultorio != null)
                {
                    try
                    {
                        _context.Consultorios.Remove(consultorio);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException ex)
                    {
                        if (ex.InnerException is SqlException sqlEx)
                        {
                            if (sqlEx.Message.Contains("FK__Cita__NumConsult__42E1EEFE"))
                            {
                                ModelState.AddModelError(string.Empty, "No se puede eliminar este consultorio porque tiene citas asociadas.");
                            }
                            else
                            {
                                _context.Consultorios.Remove(consultorio);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Ocurrió un error al eliminar el consultorio. Inténtelo de nuevo más tarde.");
                        }

                        return View("Delete", consultorio);
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }
        
        private bool ConsultorioExists(int id)
        {
            return _context.Consultorios.Any(e => e.NumConsultorio == id);
        }
    }
}
