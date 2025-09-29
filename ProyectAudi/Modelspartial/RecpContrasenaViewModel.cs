using System.ComponentModel.DataAnnotations;

public class RecpContrasenaViewModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Token { get; set; }

    [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Debe incluir mayúsculas, minúsculas, números y símbolos.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirma tu nueva contraseña.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    public string Confirmacion { get; set; }
}
