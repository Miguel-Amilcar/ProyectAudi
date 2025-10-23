using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProyectAudi.Models;

namespace ProyectAudi.ViewModels.Roles
{
    public class IndexViewModel
    {
        public int ROL_ID { get; set; }
        public string ROL_NOMBRE { get; set; } = string.Empty;
        public string? ROL_DESCRIPCION { get; set; }
        public bool ES_ADMINISTRATIVO { get; set; }
        public bool ESTADO { get; set; }
        public string CREADO_POR { get; set; } = string.Empty;
        public DateTime FECHA_CREACION { get; set; }
    }
}