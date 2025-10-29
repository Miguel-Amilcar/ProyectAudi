using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.ViewModels.Permisos
{
    public class CreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string PERMISO_NOMBRE { get; set; } = string.Empty;

        [StringLength(255)]
        public string? DESCRIPCION { get; set; }

        [Required]
        public int ROL_ID { get; set; }

        public string CREADO_POR { get; set; } = string.Empty;
        public DateTime FECHA_CREACION { get; set; }
    }
}
