using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProyectAudi.Models;

namespace ProyectAudi.ViewModels.Crear_Contraseña
{
    public class IndexViewModel
    {
        public int CREDENCIAL_ID { get; set; }
        public string USUARIO_NOMBRE { get; set; } = null!;
        public int USUARIO_ID { get; set; }
        public bool MFA_ENABLED { get; set; }
        public int INTENTOS_FALLIDOS { get; set; }
        public DateTime? BLOQUEADO_HASTA { get; set; }
        public DateTime? FECHA_ULTIMO_INTENTO { get; set; }
    }
}
