using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.Modelspartial
{
    public class IngresarTokenViewModel
    {
        [Required(ErrorMessage = "El token es obligatorio")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El token debe tener 6 dígitos")]
        [Display(Name = "Token")]
        public string Token { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Debe incluir mayúsculas, minúsculas, números y símbolos")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NuevaContrasena { get; set; } = null!;

        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare(nameof(NuevaContrasena), ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasena { get; set; } = null!;
    }


}
