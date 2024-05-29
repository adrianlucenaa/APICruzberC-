                                           Repositorio de Prácticas Profesionales
Este repositorio contiene el código desarrollado durante mis prácticas profesionales en Cruzber. El proyecto consiste en una API creada con C# y .NET 6 ,que utiliza,
autenticación mediante tokens JWT para proteger sus endpoints. Todos los endpoints están validados, tanto en los datos de entrada como en los datos de salida.

                                                  Tecnologías Utilizadas
Lenguaje de Programación: C# y .net 6

Framework: Entity Framework

Base de Datos: SQL Server

Conexión a la Base de Datos
La API utiliza Entity Framework Core para gestionar la conexión y las operaciones en la base de datos. La configuración de la base de datos se encuentra en el archivo appsettings.json.

                                                 Instalación y Configuración

Clona este repositorio

Navega al directorio del proyecto:

cd nombre_del_repositorio

Configura la cadena de conexión a la base de datos en el archivo appsettings.json.

Ejecuta las migraciones para crear la base de datos:
dotnet ef database update

Inicia el servidor:
dotnet run

La API estará disponible en http://localhost:5000.

                                                Estructura del Proyecto
Program.cs: Configuración y punto de entrada de la aplicación.

Conecction: Connecion a la base de datos.

Controllers: Controladores de la API.

Models: Modelos de datos.

Data: Contexto de la base de datos y configuración de Entity Framework.

AppSettings: Configuracion del proyecto.

                                                    Contribuciones
Las contribuciones son bienvenidas. Por favor, abre un issue para discutir cualquier cambio que te gustaría hacer.
