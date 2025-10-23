using ProyectAudi.Models;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.ViewModels.Permisos
{
    public class EditViewModel
    {
        public int PERMISO_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string PERMISO_NOMBRE { get; set; } = string.Empty;

        [StringLength(255)]
        public string? DESCRIPCION { get; set; }

        [Required]
        public int ROL_ID { get; set; }

        public string MODIFICADO_POR { get; set; } = string.Empty;
        public DateTime FECHA_MODIFICACION { get; set; }
    }
}
