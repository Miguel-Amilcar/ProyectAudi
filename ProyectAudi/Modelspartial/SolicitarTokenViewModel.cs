using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.Modelspartial
{
        public class SolicitarTokenViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Correo electrónico")]
            public string Correo { get; set; } = null!;
        }
}
