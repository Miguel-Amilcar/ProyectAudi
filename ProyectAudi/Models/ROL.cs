using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectAudi.Models
{
    [Table("ROL")]
    public class ROL
    {
        [Key]
        public int ROL_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string ROL_NOMBRE { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ROL_DESCRIPCION { get; set; }

        [Required]
        [StringLength(50)]
        public string CREADO_POR { get; set; } = string.Empty;

        [Required]
        public DateTime FECHA_CREACION { get; set; } = DateTime.UtcNow;

        // ✅ Propiedad de navegación que faltaba
        public virtual ICollection<USUARIO> USUARIOS { get; set; } = new List<USUARIO>();

        // Opcional: si usas ROL_PERMISO
        public virtual ICollection<ROL_PERMISO> ROL_PERMISOS { get; set; } = new List<ROL_PERMISO>();
    }
}
