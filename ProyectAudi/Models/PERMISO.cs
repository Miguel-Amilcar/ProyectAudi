using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectAudi.Models
{
    [Table("PERMISO")]
    public class PERMISO
    {
        [Key]
        public int PERMISO_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string NOMBRE_PERMISO { get; set; } = string.Empty;

        [StringLength(255)]
        public string? DESCRIPCION { get; set; }

        // ✅ Propiedad que faltaba
        public DateTime FECHA_CREACION { get; set; } = DateTime.UtcNow;

        // Navegación hacia ROL_PERMISO si la usas
        public virtual ICollection<ROL_PERMISO> ROL_PERMISOS { get; set; } = new List<ROL_PERMISO>();
    }
}
