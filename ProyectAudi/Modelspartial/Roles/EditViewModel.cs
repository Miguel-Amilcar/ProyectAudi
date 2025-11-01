using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.ViewModels.Roles
{
    public class EditViewModel
    {
        [Required]
        public int ROL_ID { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre del Rol")]
        public string ROL_NOMBRE { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "La descripción no puede exceder los 255 caracteres.")]
        [Display(Name = "Descripción")]
        public string? ROL_DESCRIPCION { get; set; }
    }
}
