namespace ProyectAudi.Modelspartial.Correos
{
    public class CorreoEnviadoViewModel
    {
        public int CorreoId { get; set; }
        public string Destinatario { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public DateTime FechaEnvio { get; set; }

        public string NombreRemitente { get; set; }
        public string RolRemitente { get; set; }
        public DateTime FechaCreacionRemitente { get; set; }
    }
}

