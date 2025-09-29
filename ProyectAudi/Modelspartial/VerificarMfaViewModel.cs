using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.Modelspartial
{
    public class VerificarMfaViewModel
    {
        [Required(ErrorMessage = "El código MFA es obligatorio.")]
        [Display(Name = "Código MFA")]
        public string CodigoMFA { get; set; }

        [Display(Name = "Recordar este dispositivo")]
        public bool RecordarDispositivo { get; set; }
    }
}


