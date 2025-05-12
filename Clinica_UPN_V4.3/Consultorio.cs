using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Clinica_UPN_V4._3;

public partial class Consultorio
{
    [Required(ErrorMessage = "El campo Número de Consultorio es obligatorio.")]
    [Range(1, 10, ErrorMessage = "El Número de Consultorio debe estar entre 1 y 10. No se permite letras o caracteres especiales o el número 0. ")]
    public int NumConsultorio { get; set; }

    [Required(ErrorMessage = "El campo Especialidad de Consultorio es obligatorio.")]
    //[StringLength(25, MinimumLength = 8, ErrorMessage = "La especialización debe tener exactamente 8 dígitos.")]
    [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ ]+$", ErrorMessage = "La Especialidad de Consultorio solo puede contener letras, espacios y caracteres acentuados.")]
    public string EspConsultorio { get; set; } = null!;

    public string DatosConsultorio
    {
        get { return $"{NumConsultorio} - {EspConsultorio}"; }
    }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
