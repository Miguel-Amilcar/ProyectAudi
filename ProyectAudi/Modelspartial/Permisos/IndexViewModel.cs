using ProyectAudi.Models;

namespace ProyectAudi.ViewModels.Permisos
{
    public class IndexViewModel
    {
        public int PERMISO_ID { get; set; }
        public string PERMISO_NOMBRE { get; set; } = string.Empty;
        public string? DESCRIPCION { get; set; }
        public string ROL_NOMBRE { get; set; } = string.Empty;
        public string CREADO_POR { get; set; } = string.Empty;
        public DateTime FECHA_CREACION { get; set; }
    }
}
