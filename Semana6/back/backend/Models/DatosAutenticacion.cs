namespace backend.Models
{
    /// <summary>
    /// Modelo para los datos de login del usuario
    /// </summary>
    public class DatosLogin
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
    }

    /// <summary>
    /// Modelo para la respuesta de autenticación
    /// </summary>
    public class RespuestaAutenticacion
    {
        public string Mensaje { get; set; }
        public string Usuario { get; set; }
        public string Token { get; set; }
    }

    /// <summary>
    /// Modelo para almacenar sesiones en memoria
    /// </summary>
    public class SesionUsuario
    {
        public string IdSesion { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Activa { get; set; }
    }
}
