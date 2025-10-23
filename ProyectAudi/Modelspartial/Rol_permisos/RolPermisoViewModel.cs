using System.Collections.Generic;

namespace ProyectAudi.ViewModels
{
    public class RolPermisoViewModel
    {
        public int RolId { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public List<PermisoAsignableViewModel> Permisos { get; set; } = new();
    }
}

