using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.Modelspartial
{
        public class SolicitarTokenViewModel
        {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingresa un correo electrónico válido")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; } = null!;

    }
}
