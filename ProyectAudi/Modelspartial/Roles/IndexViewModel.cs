namespace ProyectAudi.ViewModels.Roles
{
    public class IndexViewModel
    {
        public int ROL_ID { get; set; }

        public string ROL_NOMBRE { get; set; } = string.Empty;

        public string? ROL_DESCRIPCION { get; set; }

        public DateTime FECHA_CREACION { get; set; }

        public int CantidadUsuarios { get; set; }
    }
}
