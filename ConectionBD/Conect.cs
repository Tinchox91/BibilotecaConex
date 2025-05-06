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

        //Este método ejecuta un SELECT y devuelve los resultados en un DataTable.
        public DataTable EjecutarConsulta(string sql, Dictionary<string, object>? parametros = null)
        {
            // Se crea una tabla vacía que contendrá los resultados de la consulta
            var dt = new DataTable();

            // Verifica que la conexión esté abierta; si no, lanza una excepción
            if (conexion == null || conexion.State != ConnectionState.Open)
                throw new InvalidOperationException("No hay conexión abierta.");

            // Crea un comando sobre la conexión abierta
            using (var comando = conexion.CreateCommand())
            {
                // Asigna el texto de la consulta SQL al comando
                comando.CommandText = sql;

                // Si hay parámetros, se agregan al comando uno por uno
                if (parametros != null)
                {
                    foreach (var param in parametros)
                    {
                        var dbParametro = comando.CreateParameter();  // Crea un parámetro
                        dbParametro.ParameterName = param.Key;        // Nombre del parámetro
                        dbParametro.Value = param.Value;              // Valor del parámetro
                        comando.Parameters.Add(dbParametro);          // Lo agrega al comando
                    }
                }

                // Ejecuta el comando y llena el DataTable con los resultados del lector
                using (var reader = comando.ExecuteReader())
                {
                    dt.Load(reader); // Carga todas las filas que devuelve la consulta
                }
            }

            // Devuelve el DataTable con los resultados
            return dt;
        }

        public List<Dictionary<string, object>> EjecutarConsulta(string sql)
        {
            if (conexion == null || conexion.State != ConnectionState.Open)
                throw new InvalidOperationException("No hay conexión abierta.");

            var resultados = new List<Dictionary<string, object>>();

            using (var comando = conexion.CreateCommand())
            {
                comando.CommandText = sql;

                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var fila = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            fila[reader.GetName(i)] = reader.GetValue(i);
                        }
                        resultados.Add(fila);
                    }
                }
            }

            return resultados;
        }



        //Este método ejecuta comandos como INSERT, UPDATE o DELETE y devuelve cuántas filas fueron afectadas.
        public int EjecutarComando(string sql, Dictionary<string, object>? parametros = null)
        {
            // Verifica que la conexión esté abierta antes de ejecutar el comando
            if (conexion == null || conexion.State != ConnectionState.Open)
                throw new InvalidOperationException("No hay conexión abierta.");

            // Crea un comando sobre la conexión abierta
            using (var comando = conexion.CreateCommand())
            {
                // Asigna la consulta SQL al comando
                comando.CommandText = sql;

                // Si hay parámetros, los agrega al comando
                if (parametros != null)
                {
                    foreach (var param in parametros)
                    {
                        var dbParametro = comando.CreateParameter();  // Crea un nuevo parámetro
                        dbParametro.ParameterName = param.Key;        // Nombre del parámetro
                        dbParametro.Value = param.Value;              // Valor del parámetro
                        comando.Parameters.Add(dbParametro);          // Lo agrega al comando
                    }
                }

                // Ejecuta el comando y devuelve el número de filas afectadas
                return comando.ExecuteNonQuery();
            }
        }

        public int InsertarDatos(string tabla, Dictionary<string, object> datos)
        {
            if (conexion == null || conexion.State != ConnectionState.Open)
                throw new InvalidOperationException("No hay conexión abierta.");

            var columnas = string.Join(", ", datos.Keys);
            var parametros = string.Join(", ", datos.Keys.Select(k => "@" + k));

            string sql = $"INSERT INTO {tabla} ({columnas}) VALUES ({parametros})";

            using (var comando = conexion.CreateCommand())
            {
                comando.CommandText = sql;

                foreach (var dato in datos)
                {
                    var parametro = comando.CreateParameter();
                    parametro.ParameterName = "@" + dato.Key;
                    parametro.Value = dato.Value;
                    comando.Parameters.Add(parametro);
                }

                return comando.ExecuteNonQuery(); // Devuelve cuántas filas se insertaron
            }
        }


    }
}
//COMO USAR LA BIBLIOTECA? :

/*using MiBibliotecaDeClases;

var conexion = new Conexion();

// Ejemplo con SQLite
 private string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "tiendaDB.db");  
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