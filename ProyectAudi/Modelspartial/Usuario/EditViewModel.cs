using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.ViewModels.Usuario
{
    public class EditViewModel
    {
        // Identificador único
        [Required]
        public int USUARIO_ID { get; set; }

        // Identificación
        [StringLength(13, ErrorMessage = "El CUI debe tener 13 dígitos.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "El CUI debe contener solo números.")]
        public string? CUI { get; set; }

        // Nombres
        [Required(ErrorMessage = "El primer nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "No puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string PRIMERNOMBRE { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "No puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string? SEGUNDONOMBRE { get; set; }

        [StringLength(50, ErrorMessage = "No puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string? TERCERNOMBRE { get; set; }

        // Apellidos
        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "No puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string PRIMERAPELLIDO { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "No puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string? SEGUNDOAPELLIDO { get; set; }

        [StringLength(50, ErrorMessage = "No puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string? APELLIDOCASADA { get; set; }

        // Fecha de nacimiento
        [DataType(DataType.Date)]
        public DateTime? FECHA_NACIMIENTO { get; set; }

        // Contacto
        [StringLength(20, ErrorMessage = "No puede exceder los 20 caracteres.")]
        [RegularExpression(@"^(?:\+502\s?)?\d{8}$", ErrorMessage = "Debe ser un número válido de Guatemala.")]
        public string? TELEFONO { get; set; }

        [StringLength(200, ErrorMessage = "No puede exceder los 200 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9#\-\.\/, ]+$", ErrorMessage = "Solo se permiten letras, números y los caracteres especiales: # - . / , y espacios.")]
        public string? DIRECCION { get; set; }

        // Datos fiscales
        [StringLength(20, ErrorMessage = "No puede exceder los 20 caracteres.")]
        [RegularExpression(@"^[1-9]\d{7,8}$", ErrorMessage = "Debe contener 8 o 9 dígitos numéricos y no comenzar con cero.")]
        public string? PERSONA_NIT { get; set; }

        [StringLength(255, ErrorMessage = "No puede exceder los 255 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9#\-\.\/, ]+$", ErrorMessage = "Solo se permiten letras, números y los caracteres especiales: # - . / , y espacios.")]
        public string? PERSONA_DIRECCION { get; set; }

        [StringLength(20, ErrorMessage = "No puede exceder los 20 caracteres.")]
        [RegularExpression(@"^(?:\+502\s?)?\d{8}$", ErrorMessage = "Debe ser un número válido de Guatemala.")]
        public string? PERSONA_TELEFONOCASA { get; set; }

        [StringLength(20, ErrorMessage = "No puede exceder los 20 caracteres.")]
        [RegularExpression(@"^(?:\+502\s?)?\d{8}$", ErrorMessage = "Debe ser un número válido de Guatemala.")]
        public string? PERSONA_TELEFONOMOVIL { get; set; }

        // Cuenta
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(150, ErrorMessage = "No puede exceder los 150 caracteres.")]
        public string USUARIO_CORREO { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        public int ROL_ID { get; set; }

        // Archivos
        public IFormFile? FotografiaFile { get; set; }
        public IFormFile? DpiFile { get; set; }

        // Auditoría (se asigna en el controlador)
        public string MODIFICADO_POR { get; set; } = string.Empty;

        public string RolNombre { get; set; }
        //public string NombreCompleto { get; internal set; }

        public string NombreCompleto =>
    string.Join(" ", new[] {
        PRIMERNOMBRE,
        SEGUNDONOMBRE,
        TERCERNOMBRE,
        PRIMERAPELLIDO,
        SEGUNDOAPELLIDO,
        APELLIDOCASADA
    }.Where(n => !string.IsNullOrWhiteSpace(n)));

    }
}
