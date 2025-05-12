using System.ComponentModel.DataAnnotations;

namespace Clinica_UPN_V4._3.Models
{
    public class Login : Administrador
    {
        // Solo necesita UsuarioAdmin y Contraseña para el login
        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El Usuario debe tener exactamente 13 caracteres.")]
        [RegularExpression("^[a-zA-Z0-9ÁÉÍÓÚáéíóúÜüÑñ]*$", ErrorMessage = "El Usuario debe contener solo letras, números y caracteres acentuados.")]
        public new string UsuarioAdmin { get; set; } = null!;

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [StringLength(30, MinimumLength = 10, ErrorMessage = "La Contraseña debe tener minímo 10 caracteres o más.")]
        [ValidarContraseña1(ErrorMessage = "La Contraseña debe contener al menos una letra mayúscula y al menos un número.")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%&+-]).*$", ErrorMessage = "La Contraseña debe contener al menos una letra mayúscula, al menos un número y al menos un carácter especial.")]
        public new string Contraseña { get; set; } = null!;
    }
}
