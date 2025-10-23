using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProyectAudi.Models;

namespace ProyectAudi.ViewModels.Roles
{
    public class DetailsViewModel
    {
        public int ROL_ID { get; set; }
        public string ROL_NOMBRE { get; set; } = string.Empty;
        public string? ROL_DESCRIPCION { get; set; }
        public bool ES_ADMINISTRATIVO { get; set; }
        public bool ESTADO { get; set; }
        public string CREADO_POR { get; set; } = string.Empty;
        public DateTime FECHA_CREACION { get; set; }
        public string? MODIFICADO_POR { get; set; }
        public DateTime? FECHA_MODIFICACION { get; set; }
        public bool ELIMINADO { get; set; }
        public string? ELIMINADO_POR { get; set; }
        public DateTime? FECHA_ELIMINACION { get; set; }
    }
}

