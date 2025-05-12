using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinica_UPN_V4._3;

public partial class Medico
{
    [Required(ErrorMessage = "El campo DNI es obligatorio.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "El DNI debe contener solo números.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener exactamente 8 dígitos.")]
    public string Dni { get; set; } = null!;

    [Required(ErrorMessage = "El campo Apellidos es obligatorio.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Los apellidos deben tener entre 3 y 50 caracteres.")]
    [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ ]+$", ErrorMessage = "El Apellido solo puede contener letras, espacios y caracteres acentuados.")]
    public string Apellidos { get; set; } = null!;

    [Required(ErrorMessage = "El campo Nombres es obligatorio.")]
    [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ ]+$", ErrorMessage = "Elnombre solo puede contener letras, espacios y caracteres acentuados.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Los nombres deben tener entre 3 y 50 caracteres.")]
    public string Nombres { get; set; } = null!;

    [Required(ErrorMessage = "El campo Fecha de Nacimiento es obligatorio.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El campo Teléfono es obligatorio.")]
    [StringLength(9, MinimumLength = 9, ErrorMessage = "El teléfono debe tener exactamente 9 dígitos.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "El teléfono debe contener solo números.")]
    public string Telefono { get; set; } = null!;

    [Required(ErrorMessage = "El campo Número de Colegiatura es obligatorio.")]
    [RegularExpression("^[0-9]{10}$", ErrorMessage = "El Número de Colegiatura debe contener solo dígitos y tener exactamente 10 caracteres. No se permite letras o caracteres especiales.")]
    public int NumColegiatura { get; set; }

    [Required(ErrorMessage = "El campo Especialización es obligatorio.")]
    //[StringLength(30, MinimumLength = 8, ErrorMessage = "La especialización debe tener entre 8 y 30 caracteres. No se permite ingresar números o caracteres especiales. ")]
    [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ ]+$", ErrorMessage = "La Especialización solo puede contener letras, espacios y caracteres acentuados.")]
    public string Especializacion { get; set; } = null!;

    [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
    [StringLength(12, MinimumLength = 12, ErrorMessage = "El Usuario debe tener exactamente 10 caracteres.")]
    [RegularExpression("^[a-zA-Z0-9ÁÉÍÓÚáéíóúÜüÑñ]*$", ErrorMessage = "El Usuario debe contener solo letras, números y caracteres acentuados.")]
    public string UsuarioMed { get; set; } = null!;

    [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
    [StringLength(30, MinimumLength = 10, ErrorMessage = "La Contraseña debe tener minímo 10 caracteres o más.")]
    [ValidarContraseña2(ErrorMessage = "La Contraseña debe contener al menos una letra mayúscula y al menos un número.")]
    [RegularExpression("^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%&+-]).*$", ErrorMessage = "La Contraseña debe contener al menos una letra mayúscula, al menos un número y al menos un carácter especial.")]
    public string Contraseña { get; set; } = null!;

    public static async Task<List<Medico>> BuscarMedicosAsync(ClinicaUpnV4Context context, string searchString)
    {
        IQueryable<Medico> medicos = from m in context.Medicos
                                     select m;

        if (!string.IsNullOrEmpty(searchString))
        {
            medicos = medicos.Where(s => s.Nombres.Contains(searchString)
                                       || s.Apellidos.Contains(searchString)
                                       || s.NumColegiatura.ToString().Contains(searchString)
                                       || s.Especializacion.Contains(searchString));
        }

        return await medicos.ToListAsync();
    }

    // Propiedad calculada para nombre completo
    public string NombreCompletoMedico2
    {
        get { return $"{NumColegiatura} - {Nombres} {Apellidos} - {Especializacion}"; }
    }
    public string NombreCompletoMedico => $"{NumColegiatura} - {Nombres} {Apellidos}";
    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    [NotMapped]
    public bool MantenerActivo { get; set; }
}

public class ValidarContraseña2Attribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var contraseña = value as string;
        if (contraseña == null)
            return false;

        // Verificar si la contraseña contiene al menos una letra mayúscula
        return contraseña.Any(char.IsUpper) && contraseña.Any(char.IsDigit);
    }
}
