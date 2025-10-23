using System.Collections.Generic;

namespace ProyectAudi.ViewModels
{
    public class UsuarioPermisoViewModel
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;

        public int RolId { get; set; }
        public string RolNombre { get; set; } = string.Empty;

        public List<PermisoItemViewModel> Permisos { get; set; } = new();
        public List<RolItemViewModel> RolesDisponibles { get; set; } = new();
    }

    public class PermisoItemViewModel
    {
        public int PermisoId { get; set; }
        public string NombrePermiso { get; set; } = string.Empty;
        public bool Asignado { get; set; }
    }

    public class RolItemViewModel
    {
        public int RolId { get; set; }
        public string RolNombre { get; set; } = string.Empty;
    }
}
