using System;

namespace ProyectAudi.ViewModels.Usuario
{
    public class DetailsViewModel
    {
        // Identificador único
        public int USUARIO_ID { get; set; }
        public string? CUI { get; set; }

        // Nombres y apellidos
        public string PRIMERNOMBRE { get; set; } = string.Empty;
        public string? SEGUNDONOMBRE { get; set; }
        public string? TERCERNOMBRE { get; set; }

        public string PRIMERAPELLIDO { get; set; } = string.Empty;
        public string? SEGUNDOAPELLIDO { get; set; }
        public string? APELLIDOCASADA { get; set; }

        public DateTime? FECHA_NACIMIENTO { get; set; }

        // Contacto
        public string? TELEFONO { get; set; }
        public string? DIRECCION { get; set; }

        // Datos fiscales
        public string? PERSONA_NIT { get; set; }
        public string? PERSONA_DIRECCION { get; set; }
        public string? PERSONA_TELEFONOCASA { get; set; }
        public string? PERSONA_TELEFONOMOVIL { get; set; }

        // Cuenta
        public string USUARIO_CORREO { get; set; } = string.Empty;

        public byte ESTADO_TINY { get; set; }

        // Auditoría
        public string CREADO_POR { get; set; } = string.Empty;
        public DateTime? FECHA_CREACION { get; set; }

        public string? MODIFICADO_POR { get; set; }
        public DateTime? FECHA_MODIFICACION { get; set; }

        public string NombreCompleto { get; set; } = string.Empty;
        public string RolNombre { get; set; } = string.Empty;

    //    public string NombreCompleto =>
    //string.Join(" ", new[] {
    //    PRIMERNOMBRE,
    //    SEGUNDONOMBRE,
    //    TERCERNOMBRE,
    //    PRIMERAPELLIDO,
    //    SEGUNDOAPELLIDO,
    //    APELLIDOCASADA
    //}.Where(n => !string.IsNullOrWhiteSpace(n)));


    }
}
