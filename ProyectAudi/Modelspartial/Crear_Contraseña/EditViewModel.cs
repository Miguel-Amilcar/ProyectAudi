using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProyectAudi.Models;

namespace ProyectAudi.ViewModels.Crear_Contraseña
{
    public class EditViewModel
    {
        public int CREDENCIAL_ID { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Usuario de inicio de sesión")]
        public string USUARIO_NOMBRE { get; set; } = null!;

        [Required(ErrorMessage = "Debe seleccionar un usuario.")]
        [Display(Name = "Asignar a")]
        public int USUARIO_ID { get; set; }

        [Display(Name = "Intentos fallidos")]
        public int INTENTOS_FALLIDOS { get; set; }

        [Display(Name = "MFA activo")]
        public bool MFA_ENABLED { get; set; }
    }
}
