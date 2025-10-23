using ProyectAudi.Data;
using System.Collections.Generic;
using System.Linq;

namespace ProyectAudi.Services
{
    public static class PermisoService
    {
        // 🔍 Obtener todos los permisos asignados a un rol
        public static HashSet<string> ObtenerPermisos(ProyectDbContext context, int rolId)
        {
            return context.ROL_PERMISO
                .Where(rp => rp.ROL_ID == rolId)
                .Select(rp => rp.PERMISO.NOMBRE_PERMISO)
                .ToHashSet();
        }

        // ✅ Verificar si el rol tiene un permiso específico
        public static bool Tiene(HashSet<string> permisos, string nombre)
        {
            return permisos.Contains(nombre);
        }
    }
}
