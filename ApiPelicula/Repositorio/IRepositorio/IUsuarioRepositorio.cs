using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;

namespace ApiPelicula.Repositorio.IRepositorio
{

    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int id);
        bool IsUniqueUser(string usuario);
        Task<UsuarioLoginRespuestaDto> login(UsuarioLoginDto usuaioLoginDto);
        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}