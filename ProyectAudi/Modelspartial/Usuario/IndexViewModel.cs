namespace ProyectAudi.ViewModels.Usuario

{
    public class IndexViewModel
    {
        // Identificador único
        public int USUARIO_ID { get; set; }

        // Identificación
        public string? CUI { get; set; }

        // Nombre completo (ya concatenado en el controlador o servicio)
        public string NombreCompleto { get; set; } = string.Empty;

        // Contacto
        public string USUARIO_CORREO { get; set; } = string.Empty;
        public string? TELEFONO { get; set; }

        // Rol y estado
        public string? RolNombre { get; set; }

        /// <summary>
        /// Estado del usuario: 0 = Inactivo, 1 = Activo, 2 = Suspendido
        /// </summary>
        public byte ESTADO_TINY { get; set; }
    }
}

