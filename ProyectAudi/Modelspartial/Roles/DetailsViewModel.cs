using System;
using System.Collections.Generic;

namespace ProyectAudi.ViewModels.Roles
{
    public class DetailsViewModel
    {
        public int ROL_ID { get; set; }

        public string ROL_NOMBRE { get; set; } = string.Empty;

        public string? ROL_DESCRIPCION { get; set; }

        public DateTime FECHA_CREACION { get; set; }

        public string CREADO_POR { get; set; } = string.Empty;

        public List<string> UsuariosAsociados { get; set; } = new();

        public List<string> PermisosAsignados { get; set; } = new();
    }
}
