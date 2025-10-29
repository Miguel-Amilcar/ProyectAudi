using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.ViewModels.Usuario
{
    public class CreateViewModel
    {
        // Identificación
        [Required(ErrorMessage = "El CUI es obligatorio.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "El CUI debe contener exactamente 13 dígitos numéricos.")]
        public string? CUI { get; set; }

        // Nombres
        [Required(ErrorMessage = "El primer nombre es obligatorio.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Debe tener entre 2 y 30 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string PRIMERNOMBRE { get; set; } = string.Empty;

        [StringLength(30, MinimumLength = 2, ErrorMessage = "Debe tener entre 2 y 30 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string? SEGUNDONOMBRE { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "Debe tener entre 2 y 30 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string? TERCERNOMBRE { get; set; }

        // Apellidos
        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Debe tener entre 2 y 30 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string PRIMERAPELLIDO { get; set; } = string.Empty;

        [StringLength(30, MinimumLength = 2, ErrorMessage = "Debe tener entre 2 y 30 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string? SEGUNDOAPELLIDO { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "Debe tener entre 2 y 30 caracteres.")]
        [RegularExpression(@"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ\s]*$", ErrorMessage = "Debe comenzar con mayúscula y contener solo letras.")]
        public string? APELLIDOCASADA { get; set; }

        // Fecha de nacimiento

        [Required(ErrorMessage = "La fecha de nacimiento es obligatorio")]
        [DataType(DataType.Date)]
        public DateTime? FECHA_NACIMIENTO { get; set; }

        // Contacto
        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^(?:\+502\s?)?\d{8}$", ErrorMessage = "Debe ser un número válido de Guatemala 8 dígitos")]
        public string? TELEFONO { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(150, MinimumLength = 10, ErrorMessage = "Debe tener entre 10 y 150 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9#\-\.\/, ]+$", ErrorMessage = "Solo se permiten letras, números y los caracteres especiales: # - . / , y espacios.")]
        public string? DIRECCION { get; set; }

        // Datos fiscales
        [Required(ErrorMessage = "El NIT es obligatorio.")]
        [RegularExpression(@"^[1-9]\d{7,8}$", ErrorMessage = "Debe contener 8 o 9 dígitos numéricos y no comenzar con cero.")]
        public string? PERSONA_NIT { get; set; }

        [StringLength(150, MinimumLength = 10, ErrorMessage = "Debe tener entre 10 y 150 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9#\-\.\/, ]+$", ErrorMessage = "Solo se permiten letras, números y los caracteres especiales: # - . / , y espacios.")]
        public string? PERSONA_DIRECCION { get; set; }

        [RegularExpression(@"^(?:\+502\s?)?\d{8}$", ErrorMessage = "Debe ser un número válido de Guatemala.")]
        public string? PERSONA_TELEFONOCASA { get; set; }

        [RegularExpression(@"^(?:\+502\s?)?\d{8}$", ErrorMessage = "Debe ser un número válido de Guatemala.")]
        public string? PERSONA_TELEFONOMOVIL { get; set; }

        // Cuenta
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(150, ErrorMessage = "No puede exceder los 150 caracteres.")]
        public string USUARIO_CORREO { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        public int? ROL_ID { get; set; }


        // Archivos
        //[Required(ErrorMessage = "La fotografía es obligatoria.")]
        public IFormFile? FotografiaFile { get; set; }

        //[Required(ErrorMessage = "El archivo del DPI es obligatorio.")]
        public IFormFile? DpiFile { get; set; }

        // Este campo se asigna automáticamente en el controlador
        public string CREADO_POR { get; set; } = string.Empty;
        //public string RolNombre { get; set; }
        public int USUARIO_ID { get; set; }


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
