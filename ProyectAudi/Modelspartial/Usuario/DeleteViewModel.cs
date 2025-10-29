using System;

namespace ProyectAudi.ViewModels.Usuario
{
    public class DeleteViewModel
    {
        // Identificador único
        public int USUARIO_ID { get; set; }

        // Datos personales
        public string? CUI { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? USUARIO_CORREO { get; set; }
        public string? TELEFONO { get; set; }
        public string? DIRECCION { get; set; }

        // Rol y estado
        public string? RolNombre { get; set; }
        public byte ESTADO_TINY { get; set; }

        // Auditoría
        public string? CREADO_POR { get; set; }
        public DateTime? FECHA_CREACION { get; set; }  // Nullable por seguridad

        public string MODIFICADO_POR { get; set; } = string.Empty;

    }
}
