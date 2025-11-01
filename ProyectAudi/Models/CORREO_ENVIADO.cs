using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectAudi.Models
{
    public class CORREO_ENVIADO
    {
        [Key]
        public int CORREO_ID { get; set; }

        [Required]
        public string DESTINATARIO { get; set; }

        [Required]
        public string ASUNTO { get; set; }

        [Required]
        public string CUERPO { get; set; }

        public DateTime FECHA_ENVIO { get; set; }

        // 🔹 Clave foránea hacia USUARIO
        public int ENVIADO_POR_ID { get; set; }

        // 🔹 Propiedad de navegación
        [ForeignKey("ENVIADO_POR_ID")]
        public USUARIO USUARIO_REMITENTE { get; set; }
    }
}
