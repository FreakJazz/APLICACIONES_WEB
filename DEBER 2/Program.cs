using System;
using System.Collections.Generic;

namespace ClienteCRUD
{
    class Program
    {
        private static ClienteRepositorio repositorio;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            repositorio = new ClienteRepositorio();

            bool continuar = true;
            while (continuar)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        CrearCliente();
                        break;
                    case "2":
                        ListarClientes();
                        break;
                    case "3":
                        BuscarCliente();
                        break;
                    case "4":
                        ActualizarCliente();
                        break;
                    case "5":
                        EliminarCliente();
                        break;
                    case "6":
                        BuscarPorNombre();
                        break;
                    case "0":
                        continuar = false;
                        Console.WriteLine("\n¡Hasta pronto!");
                        break;
                    default:
                        Console.WriteLine("\nOpción no válida. Intenta de nuevo.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║     SISTEMA DE GESTIÓN DE CLIENTES        ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("  1. Registrar nuevo cliente");
            Console.WriteLine("  2. Ver todos los clientes");
            Console.WriteLine("  3. Buscar cliente por ID");
            Console.WriteLine("  4. Actualizar información de cliente");
            Console.WriteLine("  5. Eliminar cliente");
            Console.WriteLine("  6. Buscar clientes por nombre");
            Console.WriteLine("  0. Salir");
            Console.WriteLine();
            Console.Write("Selecciona una opción: ");
        }

        static void CrearCliente()
        {
            Console.Clear();
            Console.WriteLine("═══ REGISTRAR NUEVO CLIENTE ═══\n");

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Apellido: ");
            string apellido = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Teléfono: ");
            string telefono = Console.ReadLine();

            var cliente = new Cliente(nombre, apellido, email, telefono);

            if (!cliente.ValidarEmail())
            {
                Console.WriteLine("\n⚠️  El email ingresado no es válido.");
                return;
            }

            if (repositorio.Crear(cliente))
            {
                Console.WriteLine("\n✓ Cliente registrado exitosamente.");
            }
            else
            {
                Console.WriteLine("\n✗ No se pudo registrar el cliente.");
            }
        }

        static void ListarClientes()
        {
            Console.Clear();
            Console.WriteLine("═══ LISTA DE CLIENTES ═══\n");

            var clientes = repositorio.ObtenerTodos();

            if (clientes.Count == 0)
            {
                Console.WriteLine("No hay clientes registrados.");
                return;
            }

            Console.WriteLine($"Total de clientes: {clientes.Count}\n");
            Console.WriteLine(new string('-', 100));

            foreach (var cliente in clientes)
            {
                Console.WriteLine(cliente.ToString());
            }

            Console.WriteLine(new string('-', 100));
        }

        static void BuscarCliente()
        {
            Console.Clear();
            Console.WriteLine("═══ BUSCAR CLIENTE POR ID ═══\n");

            Console.Write("Ingresa el ID del cliente: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var cliente = repositorio.ObtenerPorId(id);

                if (cliente != null)
                {
                    Console.WriteLine("\nCliente encontrado:");
                    Console.WriteLine(new string('-', 80));
                    Console.WriteLine(cliente.ToString());
                    Console.WriteLine(new string('-', 80));
                }
                else
                {
                    Console.WriteLine("\n✗ No se encontró ningún cliente con ese ID.");
                }
            }
            else
            {
                Console.WriteLine("\n⚠️  Debes ingresar un número válido.");
            }
        }

        static void ActualizarCliente()
        {
            Console.Clear();
            Console.WriteLine("═══ ACTUALIZAR CLIENTE ═══\n");

            Console.Write("Ingresa el ID del cliente a actualizar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var cliente = repositorio.ObtenerPorId(id);

                if (cliente == null)
                {
                    Console.WriteLine("\n✗ No se encontró ningún cliente con ese ID.");
                    return;
                }

                Console.WriteLine("\nDatos actuales:");
                Console.WriteLine(cliente.ToString());
                Console.WriteLine("\nIngresa los nuevos datos (deja vacío para mantener el valor actual):\n");

                Console.Write($"Nombre [{cliente.Nombre}]: ");
                string nombre = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nombre))
                    cliente.Nombre = nombre;

                Console.Write($"Apellido [{cliente.Apellido}]: ");
                string apellido = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(apellido))
                    cliente.Apellido = apellido;

                Console.Write($"Email [{cliente.Email}]: ");
                string email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email))
                {
                    cliente.Email = email;
                    if (!cliente.ValidarEmail())
                    {
                        Console.WriteLine("\n⚠️  El email ingresado no es válido.");
                        return;
                    }
                }

                Console.Write($"Teléfono [{cliente.Telefono}]: ");
                string telefono = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(telefono))
                    cliente.Telefono = telefono;

                if (repositorio.Actualizar(cliente))
                {
                    Console.WriteLine("\n✓ Cliente actualizado exitosamente.");
                }
                else
                {
                    Console.WriteLine("\n✗ No se pudo actualizar el cliente.");
                }
            }
            else
            {
                Console.WriteLine("\n⚠️  Debes ingresar un número válido.");
            }
        }

        static void EliminarCliente()
        {
            Console.Clear();
            Console.WriteLine("═══ ELIMINAR CLIENTE ═══\n");

            Console.Write("Ingresa el ID del cliente a eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var cliente = repositorio.ObtenerPorId(id);

                if (cliente == null)
                {
                    Console.WriteLine("\n✗ No se encontró ningún cliente con ese ID.");
                    return;
                }

                Console.WriteLine("\nCliente a eliminar:");
                Console.WriteLine(cliente.ToString());
                Console.Write("\n¿Estás seguro de eliminar este cliente? (S/N): ");
                
                string confirmacion = Console.ReadLine().ToUpper();

                if (confirmacion == "S")
                {
                    if (repositorio.Eliminar(id))
                    {
                        Console.WriteLine("\n✓ Cliente eliminado exitosamente.");
                    }
                    else
                    {
                        Console.WriteLine("\n✗ No se pudo eliminar el cliente.");
                    }
                }
                else
                {
                    Console.WriteLine("\nOperación cancelada.");
                }
            }
            else
            {
                Console.WriteLine("\n⚠️  Debes ingresar un número válido.");
            }
        }

        static void BuscarPorNombre()
        {
            Console.Clear();
            Console.WriteLine("═══ BUSCAR CLIENTES POR NOMBRE ═══\n");

            Console.Write("Ingresa el nombre o apellido a buscar: ");
            string termino = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(termino))
            {
                Console.WriteLine("\n⚠️  Debes ingresar un término de búsqueda.");
                return;
            }

            var clientes = repositorio.BuscarPorNombre(termino);

            if (clientes.Count == 0)
            {
                Console.WriteLine("\n✗ No se encontraron clientes que coincidan con la búsqueda.");
                return;
            }

            Console.WriteLine($"\nSe encontraron {clientes.Count} cliente(s):\n");
            Console.WriteLine(new string('-', 100));

            foreach (var cliente in clientes)
            {
                Console.WriteLine(cliente.ToString());
            }

            Console.WriteLine(new string('-', 100));
        }
    }
}
