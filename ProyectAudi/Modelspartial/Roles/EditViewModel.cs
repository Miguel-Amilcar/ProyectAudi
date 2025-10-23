using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProyectAudi.Models;


namespace ProyectAudi.ViewModels.Roles
{
    public class EditViewModel
    {
        public int ROL_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string ROL_NOMBRE { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ROL_DESCRIPCION { get; set; }

        public bool ES_ADMINISTRATIVO { get; set; }
        public bool ESTADO { get; set; }

        public string MODIFICADO_POR { get; set; } = string.Empty;
        public DateTime FECHA_MODIFICACION { get; set; }
    }
}
