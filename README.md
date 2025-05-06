# Conexión Universal a Bases de Datos en .NET

## Descripción

Esta biblioteca de clases te permite conectar cualquier tipo de base de datos en .NET, utilizando proveedores como **SQLite**, **SQL Server**, entre otros. Todo lo que necesitas es especificar la cadena de conexión y el proveedor correspondiente.

## Funcionalidades

1. **Conectar**: Establece una conexión a la base de datos usando un proveedor dinámico.
2. **Desconectar**: Cierra la conexión abierta y limpia los recursos.
3. **Obtener Conexión**: Devuelve la conexión activa para ejecutar comandos o consultas.



Una biblioteca liviana y flexible que te permite conectarte a **cualquier base de datos compatible con ADO.NET** (como SQLite, SQL Server, MySQL, PostgreSQL, etc.), utilizando el proveedor adecuado en tiempo de ejecución.

Ideal para proyectos .NET donde necesitás una conexión desacoplada y reutilizable sin depender de un tipo específico de base de datos.

---

##  Instalación

1. Cloná o descargá este repositorio.
2. Referenciá la biblioteca en tu proyecto .NET (como proyecto o como `.dll`).
3. ¡Listo! Ya podés empezar a conectarte a cualquier base de datos soportada por ADO.NET.

---

##  Cómo usar la clase `Conect`

La clase `Conect` permite conectarte a cualquier base de datos compatible con ADO.NET usando el proveedor adecuado y una cadena de conexión.

### ✅ Pasos para usarla:

#### 1. Importá el espacio de nombres de tu biblioteca:

```csharp
using MiBibliotecaDeClases; // Reemplazá con el namespace real
```

---

#### 2. Instanciá la clase `Conect`  
📋 Copiar / ✏️ Editar

```csharp
var conexion = new Conect();
```

---

#### 3. Definí tu cadena de conexión y proveedor  
📋 Copiar / ✏️ Editar

```csharp
string cadena = "Data Source=miBase.db;";
string proveedor = "Microsoft.Data.Sqlite"; // Nombre del proveedor ADO.NET
```

---

#### 4. Conectate a la base de datos  
📋 Copiar / ✏️ Editar

```csharp
if (conexion.Conectar(cadena, proveedor, out string? error))
{
    Console.WriteLine("✅ Conectado correctamente.");
    // Podés usar conexion.ObtenerConexion() para ejecutar comandos SQL.
}
else
{
    Console.WriteLine($"❌ Error: {error}");
}
```

---

#### 5. Desconectate cuando ya no la necesites  
📋 Copiar / ✏️ Editar

```csharp
conexion.Desconectar();
```

---

### 💡 Tip adicional: Ruta dinámica a la base de datos

Si tu archivo `.db` está en la misma carpeta que el ejecutable, podés armar la ruta así:

```csharp
string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "miBase.db");
string cadena = $"Data Source={ruta};";
```

---





