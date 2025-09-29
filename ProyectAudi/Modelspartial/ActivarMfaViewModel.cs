using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.Modelspartial
{
    public class ActivarMfaViewModel
    {
        [Required]
        public string SecretoBase32 { get; set; }

        [Required(ErrorMessage = "Debes ingresar el código generado por tu app.")]
        [Display(Name = "Código de verificación")]
        public string CodigoVerificacion { get; set; }
    }
}
