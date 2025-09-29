using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.Modelspartial
{
    public class CredencialesViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Debe incluir mayúsculas, minúsculas, números y símbolos")]
        public string Contrasena { get; set; }
    }
}
