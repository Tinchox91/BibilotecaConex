# Conexión Universal a Bases de Datos en .NET

## Descripción

Esta biblioteca de clases te permite conectar cualquier tipo de base de datos en .NET, utilizando proveedores como **SQLite**, **SQL Server**, entre otros. Todo lo que necesitas es especificar la cadena de conexión y el proveedor correspondiente.

## Funcionalidades

1. **Conectar**: Establece una conexión a la base de datos usando un proveedor dinámico.
2. **Desconectar**: Cierra la conexión abierta y limpia los recursos.
3. **Obtener Conexión**: Devuelve la conexión activa para ejecutar comandos o consultas.

## Código

### `Conect` - Clase principal para gestionar la conexión

```csharp
using System.Data.Common;
using System.Data;

namespace ConectionBD
{
    public class Conect
    {
        private DbConnection? conexion;

        public bool Conectar(string cadenaConexion, string proveedor, out string? mensajeError)
        {
            mensajeError = null;
            try
            {
                var factory = DbProviderFactories.GetFactory(proveedor);
                conexion = factory.CreateConnection();
                if (conexion == null)
                {
                    mensajeError = "No se pudo crear la conexión con el proveedor especificado.";
                    return false;
                }

                conexion.ConnectionString = cadenaConexion;
                conexion.Open();
                return true;
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                return false;
            }
        }

        public void Desconectar()
        {
            if (conexion != null && conexion.State == ConnectionState.Open)
            {
                conexion.Close();
                conexion.Dispose();
                conexion = null;
            }
        }

        public DbConnection? ObtenerConexion()
        {
            return conexion;
        }
    }
}
