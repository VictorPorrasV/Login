using Login.Models;

namespace Login.Repositorio.Interfaces
{
    public interface IUsuarioService
    {

        Task<Usuario> GetUsuario(string correo, string clave);
        Task<UsuarioVM> SaveUsuario(UsuarioVM modelo);

    }
}
