using System.Data.Common;
using System.Data;

namespace ConectionBD
{
    public class Conect
    {
        // Objeto que representará la conexión activa (puede ser SQLite, SQL Server, etc.)
        private DbConnection? conexion;

        // Método que conecta a la base de datos, recibiendo la cadena de conexión y el proveedor (SQLite, SqlClient, etc.)
        public bool Conectar(string cadenaConexion, string proveedor, out string? mensajeError)
        {
            mensajeError = null;
            try
            {
                // Se obtiene el proveedor de base de datos en tiempo de ejecución
                var factory = DbProviderFactories.GetFactory(proveedor);

                // Se crea una conexión usando el factory del proveedor especificado
                conexion = factory.CreateConnection();
                if (conexion == null)
                {
                    mensajeError = "No se pudo crear la conexión con el proveedor especificado.";
                    return false;
                }

                // Se asigna la cadena de conexión al objeto de conexión
                conexion.ConnectionString = cadenaConexion;

                // Se abre la conexión
                conexion.Open();
                return true; // Conexión exitosa
            }
            catch (Exception ex)
            {
                // Si hay algún error, se devuelve en mensajeError
                mensajeError = ex.Message;
                return false;
            }
        }

        // Método para cerrar y limpiar la conexión si está abierta
        public void Desconectar()
        {
            if (conexion != null && conexion.State == ConnectionState.Open)
            {
                conexion.Close();       // Cierra la conexión
                conexion.Dispose();     // Libera recursos
                conexion = null;        // Libera referencia
            }
        }

        // Método público para obtener la conexión actual (útil para ejecutar comandos, etc.)
        public DbConnection? ObtenerConexion()
        {
            return conexion;
        }

    }
}
//COMO USAR LA BIBLIOTECA? :

/*using MiBibliotecaDeClases;

var conexion = new Conexion();

// Ejemplo con SQLite
string cadena = "Data Source=miBase.db;";
string proveedor = "Microsoft.Data.Sqlite"; // Nombre del provider

if (conexion.Conectar(cadena, proveedor, out string? error))
{
    Console.WriteLine("Conectado correctamente.");
}
else
{
    Console.WriteLine($"Error: {error}");
}

*/


// PARA AGREGAR LA RUTA:

/*
 string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "miBase.db");
string cadena = $"Data Source={ruta};";
*/