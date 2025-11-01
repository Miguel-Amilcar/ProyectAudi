namespace ProyectAudi.Modelspartial.Correos
{
    public class RedactarCorreoViewModel
    {
        public string NombreCompleto { get; set; } // 👈 Esta es la propiedad que falta
        public string Destinatario { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
    }
}
