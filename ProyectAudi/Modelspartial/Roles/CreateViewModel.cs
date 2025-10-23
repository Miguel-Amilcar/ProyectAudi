using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProyectAudi.Models;

namespace ProyectAudi.ViewModels.Roles
{
    public class CreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string ROL_NOMBRE { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ROL_DESCRIPCION { get; set; }
        [Required(ErrorMessage = "Debes seleccionar si es administrativo.")]
        public bool? ES_ADMINISTRATIVO { get; set; }

        [Required(ErrorMessage = "Debes seleccionar el estado.")]
        public bool? ESTADO { get; set; }


        public string CREADO_POR { get; set; } = string.Empty;
        public DateTime FECHA_CREACION { get; set; }
    }
}
