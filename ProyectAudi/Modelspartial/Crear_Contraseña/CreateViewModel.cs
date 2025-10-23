using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProyectAudi.Models;

namespace ProyectAudi.ViewModels.Crear_Contraseña
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Usuario de inicio de sesión")]
        public string USUARIO_NOMBRE { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña inicial")]
        public string PlainPassword { get; set; } = null!;

        [Required(ErrorMessage = "Debe seleccionar un usuario.")]
        [Display(Name = "Asignar a")]
        public int USUARIO_ID { get; set; }

        // Solo para mostrar en la vista, no se envía al controlador
        public string? NombreCompletoUsuario { get; set; }
    }
}

