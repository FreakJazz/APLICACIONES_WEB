using System;
using System.Data.SQLite;
using System.IO;

namespace ClienteCRUD
{
    public class DatabaseManager
    {
        private string rutaBaseDatos;
        private string cadenaConexion;

        public DatabaseManager()
        {
            rutaBaseDatos = Path.Combine(Directory.GetCurrentDirectory(), "clientes.db");
            cadenaConexion = $"Data Source={rutaBaseDatos};Version=3;";
            InicializarBaseDatos();
        }

        private void InicializarBaseDatos()
        {
            if (!File.Exists(rutaBaseDatos))
            {
                SQLiteConnection.CreateFile(rutaBaseDatos);
            }

            using (var conexion = new SQLiteConnection(cadenaConexion))
            {
                conexion.Open();
                string crearTabla = @"
                    CREATE TABLE IF NOT EXISTS Clientes (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Apellido TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        Telefono TEXT,
                        FechaRegistro TEXT NOT NULL
                    )";
                
                using (var comando = new SQLiteCommand(crearTabla, conexion))
                {
                    comando.ExecuteNonQuery();
                }
            }
        }

        public SQLiteConnection ObtenerConexion()
        {
            var conexion = new SQLiteConnection(cadenaConexion);
            conexion.Open();
            return conexion;
        }
    }
}
