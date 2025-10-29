namespace ProyectAudi.Modelspartial.Ver_contraseña
{
    public class VerContraseñaViewModel
    {
        public int CREDENCIAL_ID { get; set; }
        public int USUARIO_ID { get; set; }
        public string USUARIO_NOMBRE { get; set; }
        public string CONTRASENA { get; set; } // ✅ Desencriptada
        public DateTime? PASSWORD_ULTIMO_CAMBIO { get; set; }
        public bool MFA_ENABLED { get; set; }
        public string ULTIMA_IP { get; set; }
        public string ULTIMO_USER_AGENT { get; set; }
    }
}
