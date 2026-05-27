using System;

namespace ClienteCRUD
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Cliente()
        {
            FechaRegistro = DateTime.Now;
        }

        public Cliente(string nombre, string apellido, string email, string telefono)
        {
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Telefono = telefono;
            FechaRegistro = DateTime.Now;
        }

        public string ObtenerNombreCompleto()
        {
            return $"{Nombre} {Apellido}";
        }

        public bool ValidarEmail()
        {
            if (string.IsNullOrEmpty(Email))
                return false;
            
            return Email.Contains("@") && Email.Contains(".");
        }

        public override string ToString()
        {
            return $"ID: {Id} | {ObtenerNombreCompleto()} | Email: {Email} | Tel: {Telefono} | Registrado: {FechaRegistro.ToShortDateString()}";
        }
    }
}
