using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.ViewModels.Roles
{
    public class DeleteViewModel
    {
        [Required]
        public int ROL_ID { get; set; }

        [Display(Name = "Nombre del Rol")]
        public string ROL_NOMBRE { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        public string? ROL_DESCRIPCION { get; set; }

        [Display(Name = "Usuarios asociados")]
        public int CantidadUsuarios { get; set; }

        [Display(Name = "Permisos asignados")]
        public int CantidadPermisos { get; set; }
    }
}
