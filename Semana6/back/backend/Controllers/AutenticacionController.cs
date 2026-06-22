using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using backend.Models;

namespace backend.Controllers
{
    /// <summary>
    /// Servicio de autenticación - Gestiona login, logout y sesiones
    /// </summary>
    public class AutenticacionServicio
    {
        // Diccionario para almacenar sesiones activas en memoria
        private static readonly Dictionary<string, SesionUsuario> sesionesActivas = 
            new Dictionary<string, SesionUsuario>();

        // Usuarios de prueba (en producción usar base de datos)
        private static readonly List<(string usuario, string contrasena)> usuariosValidos = 
            new List<(string, string)>
            {
                ("admin", "admin123"),
                ("usuario", "usuario123"),
                ("test", "test123")
            };

        /// <summary>
        /// Verifica si las credenciales son válidas
        /// </summary>
        public static bool VerificarCredenciales(string usuario, string contrasena)
        {
            return usuariosValidos.Any(u => u.usuario == usuario && u.contrasena == contrasena);
        }

        /// <summary>
        /// Crea una nueva sesión para el usuario
        /// </summary>
        public static string CrearSesion(string usuario)
        {
            var idSesion = Guid.NewGuid().ToString();
            var ahora = DateTime.UtcNow;
            var expiracion = ahora.AddHours(1);

            var sesion = new SesionUsuario
            {
                IdSesion = idSesion,
                Usuario = usuario,
                FechaCreacion = ahora,
                FechaExpiracion = expiracion,
                Activa = true
            };

            sesionesActivas[idSesion] = sesion;
            return idSesion;
        }

        /// <summary>
        /// Verifica si una sesión es válida
        /// </summary>
        public static SesionUsuario VerificarSesion(string idSesion)
        {
            if (sesionesActivas.TryGetValue(idSesion, out var sesion))
            {
                // Verificar que no esté expirada
                if (sesion.FechaExpiracion > DateTime.UtcNow && sesion.Activa)
                {
                    return sesion;
                }
                else
                {
                    // Sesión expirada, eliminarla
                    sesionesActivas.Remove(idSesion);
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Cierra una sesión del usuario
        /// </summary>
        public static void CerrarSesion(string idSesion)
        {
            if (sesionesActivas.ContainsKey(idSesion))
            {
                sesionesActivas.Remove(idSesion);
            }
        }
    }

    /// <summary>
    /// Controlador de Autenticación - Maneja login, logout y verificación de sesión
    /// </summary>
    [ApiController]
    [Route("api/autenticacion")]
    public class AutenticacionController : ControllerBase
    {
        private readonly ILogger<AutenticacionController> _logger;
        private const string NombreCookie = "sessionId";

        public AutenticacionController(ILogger<AutenticacionController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// POST: /api/autenticacion/login
        /// Realiza el login del usuario con usuario y contraseña
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] DatosLogin datos)
        {
            try
            {
                // Validar que los datos no estén vacíos
                if (string.IsNullOrWhiteSpace(datos?.Usuario) || string.IsNullOrWhiteSpace(datos?.Contrasena))
                {
                    var respuestaError = new RespuestaAutenticacion
                    {
                        Mensaje = "Usuario y contraseña son requeridos",
                        Usuario = "",
                        Token = ""
                    };
                    return BadRequest(respuestaError);
                }

                // Verificar credenciales
                if (AutenticacionServicio.VerificarCredenciales(datos.Usuario, datos.Contrasena))
                {
                    // Crear sesión
                    var idSesion = AutenticacionServicio.CrearSesion(datos.Usuario);

                    // Crear cookie HttpOnly
                    var opciones = new CookieOptions
                    {
                        HttpOnly = true,           // No accesible desde JavaScript
                        Secure = false,            // En producción: true (HTTPS)
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    };

                    Response.Cookies.Append(NombreCookie, idSesion, opciones);

                    _logger.LogInformation($"Login exitoso para usuario: {datos.Usuario}");

                    var respuesta = new RespuestaAutenticacion
                    {
                        Mensaje = "Login exitoso",
                        Usuario = datos.Usuario,
                        Token = idSesion
                    };

                    return Ok(respuesta);
                }
                else
                {
                    _logger.LogWarning($"Login fallido para usuario: {datos.Usuario}");

                    var respuestaFallo = new RespuestaAutenticacion
                    {
                        Mensaje = "Credenciales inválidas",
                        Usuario = "",
                        Token = ""
                    };

                    return Unauthorized(respuestaFallo);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en login");

                var respuestaError = new RespuestaAutenticacion
                {
                    Mensaje = "Error interno del servidor",
                    Usuario = "",
                    Token = ""
                };

                return StatusCode(StatusCodes.Status500InternalServerError, respuestaError);
            }
        }

        /// <summary>
        /// GET: /api/autenticacion/verificar-sesion
        /// Verifica si la sesión actual del usuario es válida
        /// </summary>
        [HttpGet("verificar-sesion")]
        public IActionResult VerificarSesion()
        {
            try
            {
                // Obtener cookie de sesión
                if (Request.Cookies.TryGetValue(NombreCookie, out var idSesion))
                {
                    // Verificar sesión
                    var sesion = AutenticacionServicio.VerificarSesion(idSesion);

                    if (sesion != null)
                    {
                        var respuesta = new RespuestaAutenticacion
                        {
                            Mensaje = "Sesión válida",
                            Usuario = sesion.Usuario,
                            Token = idSesion
                        };
                        return Ok(respuesta);
                    }
                    else
                    {
                        var respuestaFallo = new RespuestaAutenticacion
                        {
                            Mensaje = "Sesión no válida",
                            Usuario = "",
                            Token = ""
                        };
                        return Unauthorized(respuestaFallo);
                    }
                }
                else
                {
                    var respuestaError = new RespuestaAutenticacion
                    {
                        Mensaje = "No hay sesión activa",
                        Usuario = "",
                        Token = ""
                    };
                    return Unauthorized(respuestaError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando sesión");

                var respuestaError = new RespuestaAutenticacion
                {
                    Mensaje = "Error interno del servidor",
                    Usuario = "",
                    Token = ""
                };

                return StatusCode(StatusCodes.Status500InternalServerError, respuestaError);
            }
        }

        /// <summary>
        /// POST: /api/autenticacion/logout
        /// Cierra la sesión del usuario
        /// </summary>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                // Obtener cookie de sesión
                if (Request.Cookies.TryGetValue(NombreCookie, out var idSesion))
                {
                    // Cerrar sesión
                    AutenticacionServicio.CerrarSesion(idSesion);
                }

                // Limpiar cookie
                Response.Cookies.Delete(NombreCookie);

                _logger.LogInformation("Logout exitoso");

                var respuesta = new RespuestaAutenticacion
                {
                    Mensaje = "Logout exitoso",
                    Usuario = "",
                    Token = ""
                };

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en logout");

                var respuestaError = new RespuestaAutenticacion
                {
                    Mensaje = "Error interno del servidor",
                    Usuario = "",
                    Token = ""
                };

                return StatusCode(StatusCodes.Status500InternalServerError, respuestaError);
            }
        }
    }
}
