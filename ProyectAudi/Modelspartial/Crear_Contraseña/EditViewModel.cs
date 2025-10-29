using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProyectAudi.Models;

namespace ProyectAudi.ViewModels.Crear_Contraseña
{
    public class EditViewModel
    {

        [Required(ErrorMessage = "El nombre actual de usuario es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Usuario actual")]
        public string USUARIO_NOMBRE_ACTUAL { get; set; } = null!;

        [Required(ErrorMessage = "El nuevo nombre de usuario es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Nuevo usuario")]
        public string USUARIO_NOMBRE_NUEVO { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña actual es obligatoria")]
        [DataType(DataType.Password)]
        public string ContraseñaActual { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Debe incluir mayúsculas, minúsculas, números y símbolos")]
        [DataType(DataType.Password)]
        public string NuevaContraseña { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación es obligatoria")]
        [Compare("NuevaContraseña", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        public string ConfirmacionContraseña { get; set; } = string.Empty;

        public string MODIFICADO_POR { get; set; } = string.Empty;

        public int CREDENCIAL_ID { get; set; }

    }
}
