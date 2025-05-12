using System;
using System.ComponentModel.DataAnnotations;

namespace Clinica_UPN_V4._3
{
    public partial class Citum
    {
        [Key]
        public int NumCita { get; set; }

        public string? UsuarioMedico { get; set; }

        public string? UsuarioPaciente { get; set; }

        public DateTime? Fecha { get; set; }

        [Display(Name = "Fecha")]
        public string? FechaSoloFecha
        {
            get => Fecha?.ToString("yyyy-MM-dd");
            set
            {
                if (DateTime.TryParse(value, out DateTime parsedDate))
                {
                    if (Fecha.HasValue)
                    {
                        Fecha = parsedDate.Date + Fecha.Value.TimeOfDay;
                    }
                    else
                    {
                        Fecha = parsedDate.Date;
                    }
                }
                else
                {
                    Fecha = null;
                }
            }
        }

        [Display(Name = "Hora")]
        public string? FechaSoloHora
        {
            get => Fecha?.ToString("HH:mm");
            set
            {
                if (TimeSpan.TryParse(value, out TimeSpan parsedTime))
                {
                    if (Fecha.HasValue)
                    {
                        Fecha = Fecha.Value.Date + parsedTime;
                    }
                    else
                    {
                        Fecha = DateTime.Today + parsedTime;
                    }
                }
                else
                {
                    Fecha = null;
                }
            }
        }

        public int? NumConsultorio { get; set; }

        // Relaciones de navegación
        public virtual Consultorio? NumConsultorioNavigation { get; set; }

        public virtual Medico? UsuarioMedicoNavigation { get; set; }

        public virtual Paciente? UsuarioPacienteNavigation { get; set; }
    }
}
