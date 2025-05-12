using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Clinica_UPN_V4._3;

public partial class Administrador
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
    [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ ]+$", ErrorMessage = "El Nombre solo puede contener letras, espacios y caracteres acentuados.")]
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

    [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "El Usuario debe tener exactamente 13 caracteres.")]
    [RegularExpression("^[a-zA-Z0-9ÁÉÍÓÚáéíóúÜüÑñ]*$", ErrorMessage = "El Usuario debe contener solo letras, números y caracteres acentuados.")]
    public string UsuarioAdmin { get; set; } = null!;

    [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
    [StringLength(30, MinimumLength = 10, ErrorMessage = "La Contraseña debe tener minímo 10 caracteres o más.")]
    [ValidarContraseña1(ErrorMessage = "La Contraseña debe contener al menos una letra mayúscula y al menos un número.")]
    [RegularExpression("^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%&+-]).*$", ErrorMessage = "La Contraseña debe contener al menos una letra mayúscula, al menos un número y al menos un carácter especial.")]
    public string Contraseña { get; set; } = null!;

    [Required(ErrorMessage = "El campo Código de Seguridad es obligatorio.")]
    [RegularExpression("^[0-9]{4}$", ErrorMessage = "El Código de Seguridad debe contener solo dígitos y tener exactamente 4 caracteres.")]
    public int CodigoSeguridad { get; set; }

    [NotMapped]
    public bool MantenerActivo { get; set; }
}

public class ValidarContraseña1Attribute : ValidationAttribute
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