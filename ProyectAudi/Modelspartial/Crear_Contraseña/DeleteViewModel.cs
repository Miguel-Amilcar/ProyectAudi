using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProyectAudi.Models;

namespace ProyectAudi.ViewModels.Crear_Contraseña
{
    public class DeleteViewModel
    {
        public int CREDENCIAL_ID { get; set; }
        public string USUARIO_NOMBRE { get; set; } = null!;
        public DateTime? PASSWORD_ULTIMO_CAMBIO { get; set; }
        public bool MFA_ENABLED { get; set; }
        public DateTime? MFA_ULTIMO_USO { get; set; }
        public string? ULTIMA_IP { get; set; }
        public string? ULTIMO_USER_AGENT { get; set; }

    }
}
