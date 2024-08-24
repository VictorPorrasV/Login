using Dapper;
using Login.Models;
using Login.Repositorio.Interfaces;
using Microsoft.Data.SqlClient;

namespace Login.Repositorio.Servicios
{
    public class UsuarioService:IUsuarioService
    {

        private readonly string connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Conexion");
        }
        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM USUARIO WHERE Correo = @Correo AND Clave = @Clave";

                var usuarioEncontrado = await connection.QueryFirstOrDefaultAsync<Usuario>(
                    query,
                    new { Correo = correo, Clave = clave }
                );

                return usuarioEncontrado;
            }
        }
        public async Task<UsuarioVM> SaveUsuario(UsuarioVM modelo)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"
            INSERT INTO USUARIO (NombreCompleto, Correo, Clave)
            VALUES (@NombreCompleto, @Correo, @Clave);
            SELECT CAST(SCOPE_IDENTITY() as int);";

                // Ejecutar la consulta e insertar el usuario
                var idUsuario = await connection.ExecuteScalarAsync<int>(
                    query,
                    new
                    {
                        NombreCompleto = modelo.NombreCompleto,
                        Correo = modelo.Correo,
                        Clave = modelo.Clave
                       
                    }
                );

                // Asignar el Id generado al modelo
                modelo.IdUsuario = idUsuario;

                return modelo;
            }
        }


    }
}
