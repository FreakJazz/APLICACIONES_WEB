using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace ClienteCRUD
{
    public class ClienteRepositorio
    {
        private DatabaseManager dbManager;

        public ClienteRepositorio()
        {
            dbManager = new DatabaseManager();
        }

        public bool Crear(Cliente cliente)
        {
            try
            {
                using (var conexion = dbManager.ObtenerConexion())
                {
                    string query = @"INSERT INTO Clientes (Nombre, Apellido, Email, Telefono, FechaRegistro) 
                                   VALUES (@nombre, @apellido, @email, @telefono, @fecha)";
                    
                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
                        comando.Parameters.AddWithValue("@apellido", cliente.Apellido);
                        comando.Parameters.AddWithValue("@email", cliente.Email);
                        comando.Parameters.AddWithValue("@telefono", cliente.Telefono ?? "");
                        comando.Parameters.AddWithValue("@fecha", cliente.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss"));
                        
                        return comando.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear cliente: {ex.Message}");
                return false;
            }
        }

        public List<Cliente> ObtenerTodos()
        {
            var clientes = new List<Cliente>();
            
            try
            {
                using (var conexion = dbManager.ObtenerConexion())
                {
                    string query = "SELECT * FROM Clientes ORDER BY Id";
                    
                    using (var comando = new SQLiteCommand(query, conexion))
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            clientes.Add(new Cliente
                            {
                                Id = Convert.ToInt32(lector["Id"]),
                                Nombre = lector["Nombre"].ToString(),
                                Apellido = lector["Apellido"].ToString(),
                                Email = lector["Email"].ToString(),
                                Telefono = lector["Telefono"].ToString(),
                                FechaRegistro = DateTime.Parse(lector["FechaRegistro"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
            }
            
            return clientes;
        }

        public Cliente ObtenerPorId(int id)
        {
            try
            {
                using (var conexion = dbManager.ObtenerConexion())
                {
                    string query = "SELECT * FROM Clientes WHERE Id = @id";
                    
                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@id", id);
                        
                        using (var lector = comando.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                return new Cliente
                                {
                                    Id = Convert.ToInt32(lector["Id"]),
                                    Nombre = lector["Nombre"].ToString(),
                                    Apellido = lector["Apellido"].ToString(),
                                    Email = lector["Email"].ToString(),
                                    Telefono = lector["Telefono"].ToString(),
                                    FechaRegistro = DateTime.Parse(lector["FechaRegistro"].ToString())
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener cliente: {ex.Message}");
            }
            
            return null;
        }

        public bool Actualizar(Cliente cliente)
        {
            try
            {
                using (var conexion = dbManager.ObtenerConexion())
                {
                    string query = @"UPDATE Clientes 
                                   SET Nombre = @nombre, Apellido = @apellido, 
                                       Email = @email, Telefono = @telefono 
                                   WHERE Id = @id";
                    
                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@id", cliente.Id);
                        comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
                        comando.Parameters.AddWithValue("@apellido", cliente.Apellido);
                        comando.Parameters.AddWithValue("@email", cliente.Email);
                        comando.Parameters.AddWithValue("@telefono", cliente.Telefono ?? "");
                        
                        return comando.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar cliente: {ex.Message}");
                return false;
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                using (var conexion = dbManager.ObtenerConexion())
                {
                    string query = "DELETE FROM Clientes WHERE Id = @id";
                    
                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@id", id);
                        return comando.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar cliente: {ex.Message}");
                return false;
            }
        }

        public List<Cliente> BuscarPorNombre(string termino)
        {
            var clientes = new List<Cliente>();
            
            try
            {
                using (var conexion = dbManager.ObtenerConexion())
                {
                    string query = @"SELECT * FROM Clientes 
                                   WHERE Nombre LIKE @termino OR Apellido LIKE @termino 
                                   ORDER BY Nombre";
                    
                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@termino", $"%{termino}%");
                        
                        using (var lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                clientes.Add(new Cliente
                                {
                                    Id = Convert.ToInt32(lector["Id"]),
                                    Nombre = lector["Nombre"].ToString(),
                                    Apellido = lector["Apellido"].ToString(),
                                    Email = lector["Email"].ToString(),
                                    Telefono = lector["Telefono"].ToString(),
                                    FechaRegistro = DateTime.Parse(lector["FechaRegistro"].ToString())
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar clientes: {ex.Message}");
            }
            
            return clientes;
        }
    }
}
