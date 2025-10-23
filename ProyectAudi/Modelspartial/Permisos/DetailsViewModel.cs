using System.ComponentModel.DataAnnotations;

namespace ProyectAudi.ViewModels.Permisos
{
    public class DetailsViewModel
    {
        public int PERMISO_ID { get; set; }
        public string PERMISO_NOMBRE { get; set; } = string.Empty;
        public string? DESCRIPCION { get; set; }
        public string ROL_NOMBRE { get; set; } = string.Empty;
        public string CREADO_POR { get; set; } = string.Empty;
        public DateTime FECHA_CREACION { get; set; }
        public string? MODIFICADO_POR { get; set; }
        public DateTime? FECHA_MODIFICACION { get; set; }
        public bool ELIMINADO { get; set; }
        public string? ELIMINADO_POR { get; set; }
        public DateTime? FECHA_ELIMINACION { get; set; }
    }
}
