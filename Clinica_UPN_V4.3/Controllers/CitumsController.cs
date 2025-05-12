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
    
    public class CitumsController : Controller
    {
        private readonly ClinicaUpnV4Context _context;

        public CitumsController(ClinicaUpnV4Context context)
        {
            _context = context;
        }

        // GET: Citums
        public async Task<IActionResult> Index()
        {
            var clinicaUpnV4Context = _context.Cita.Include(c => c.NumConsultorioNavigation).Include(c => c.UsuarioMedicoNavigation).Include(c => c.UsuarioPacienteNavigation);
            return View(await clinicaUpnV4Context.ToListAsync());
        }

        // GET: Citums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citum = await _context.Cita
                .Include(c => c.NumConsultorioNavigation)
                .Include(c => c.UsuarioMedicoNavigation)
                .Include(c => c.UsuarioPacienteNavigation)
                .FirstOrDefaultAsync(m => m.NumCita == id);
            if (citum == null)
            {
                return NotFound();
            }

            return View(citum);
        }

        // GET: Citums/Create
        /*
        public IActionResult Create()
        {
            ViewData["NumConsultorio"] = new SelectList(_context.Consultorios, "NumConsultorio", "EspConsultorio");
            ViewData["UsuarioMedico"] = new SelectList(_context.Medicos, "UsuarioMed", "NombreCompletoMedico");
            ViewData["UsuarioPaciente"] = new SelectList(_context.Pacientes, "UsuarioPac", "NombreCompleto");
            return View();
        }
        */

        public async Task<IActionResult> Create(string searchString, int? consultorioId)
        {
            var medicos = await Medico.BuscarMedicosAsync(_context, searchString);

            var nombresCompletos = medicos.Select(m => m.NombreCompletoMedico2).ToList();

            ViewData["NumConsultorio"] = new SelectList(_context.Consultorios, "NumConsultorio", "DatosConsultorio");

            if (consultorioId.HasValue)
            {
                var especializacionConsultorio = await _context.Consultorios
                                                    .Where(c => c.NumConsultorio == consultorioId)
                                                    .Select(c => c.EspConsultorio)
                                                    .FirstOrDefaultAsync();

                ViewData["UsuarioMedico"] = new SelectList(
                    _context.Medicos.Where(m => m.Especializacion == especializacionConsultorio),
                    "UsuarioMed",
                    "NombreCompletoMedico2"
                );
            }
            else
            {
                ViewData["UsuarioMedico"] = new SelectList(_context.Medicos, "UsuarioMed", "NombreCompletoMedico2");
            }

            ViewData["UsuarioPaciente"] = new SelectList(_context.Pacientes, "UsuarioPac", "NombreCompleto");
            ViewData["NombresCompletosMedicos"] = nombresCompletos;

            return View();
        }

        public async Task<IActionResult> GetAvailableHours(string usuarioMed, string fecha)
        {
            if (!DateTime.TryParse(fecha, out DateTime parsedDate))
            {
                return BadRequest("Fecha inválida");
            }

            try
            {
                
                var citas = await _context.Cita
                    .Where(c => c.UsuarioMedico == usuarioMed && c.Fecha.HasValue && c.Fecha.Value.Date == parsedDate.Date)
                    .Select(c => c.FechaSoloHora)
                    .ToListAsync();

                
                var horas = new List<string>
                {
                    "08:00", "08:20", "08:40", "09:00", "09:20", "09:40", "10:00", "10:20", "10:40",
                    "11:00", "11:20", "11:40", "12:00", "12:20", "12:40", "13:00", "13:20", "13:40",
                    "14:00", "14:20", "14:40", "15:00", "15:20", "15:40", "16:00", "16:20", "16:40",
                    "17:00", "17:20", "17:40", "18:00", "18:20", "18:40", "19:00", "19:20", "19:40"
                };

                
                var horasDisponibles = horas.Except(citas).ToList();

                return Json(horasDisponibles);
            }
            catch (Exception ex)
            {
                
                return BadRequest($"Error al obtener las horas disponibles: {ex.Message}");
            }
        }

        // POST: Citums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumCita,UsuarioMedico,UsuarioPaciente,FechaSoloFecha,FechaSoloHora,NumConsultorio")] Citum citum)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(citum);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlEx)
                    {
                        if (sqlEx.Message.Contains("unique_fecha_evento"))
                        {
                            ModelState.AddModelError("Fecha", "la fecha de la cita debe ser única dentro del rango habilitado");
                        }
                        else
                        {
                            _context.Add(citum);
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
            ViewData["NumConsultorio"] = new SelectList(_context.Consultorios, "NumConsultorio", "NumConsultorio", citum.NumConsultorio);
            ViewData["UsuarioMedico"] = new SelectList(_context.Medicos, "UsuarioMed", "UsuarioMed", citum.UsuarioMedico);
            ViewData["UsuarioPaciente"] = new SelectList(_context.Pacientes, "UsuarioPac", "UsuarioPac", citum.UsuarioPaciente);
            return View(citum);
        }

        // GET: Citums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citum = await _context.Cita.FindAsync(id);
            if (citum == null)
            {
                return NotFound();
            }
            ViewData["NumConsultorio"] = new SelectList(_context.Consultorios, "NumConsultorio", "EspConsultorio", citum.NumConsultorio);
            ViewData["UsuarioMedico"] = new SelectList(_context.Medicos, "UsuarioMed", "NombreCompletoMedico", citum.UsuarioMedico);
            ViewData["UsuarioPaciente"] = new SelectList(_context.Pacientes, "UsuarioPac", "NombreCompleto", citum.UsuarioPaciente);
            return View(citum);
        }

        // POST: Citums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumCita,UsuarioMedico,UsuarioPaciente,Fecha,NumConsultorio")] Citum citum)
        {
            if (id != citum.NumCita)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitumExists(citum.NumCita))
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
            ViewData["NumConsultorio"] = new SelectList(_context.Consultorios, "NumConsultorio", "NumConsultorio", citum.NumConsultorio);
            ViewData["UsuarioMedico"] = new SelectList(_context.Medicos, "UsuarioMed", "UsuarioMed", citum.UsuarioMedico);
            ViewData["UsuarioPaciente"] = new SelectList(_context.Pacientes, "UsuarioPac", "UsuarioPac", citum.UsuarioPaciente);
            return View(citum);
        }

        // GET: Citums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citum = await _context.Cita
                .Include(c => c.NumConsultorioNavigation)
                .Include(c => c.UsuarioMedicoNavigation)
                .Include(c => c.UsuarioPacienteNavigation)
                .FirstOrDefaultAsync(m => m.NumCita == id);
            if (citum == null)
            {
                return NotFound();
            }

            return View(citum);
        }

        // POST: Citums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var citum = await _context.Cita.FindAsync(id);
            if (citum != null)
            {
                _context.Cita.Remove(citum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitumExists(int id)
        {
            return _context.Cita.Any(e => e.NumCita == id);
        }
    }
}
