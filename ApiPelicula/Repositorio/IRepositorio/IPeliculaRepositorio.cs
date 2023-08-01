using ApiPelicula.Model;

namespace ApiPelicula.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        ICollection<Pelicula> GetPelicula();
        Pelicula GetPelicula(int id);
        bool ExistePelicula(string nombre);
        bool ExistePelicula(int id);
        bool CrearPelicula(Pelicula pelicula);
        bool ActualizarPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);
        ICollection<Pelicula> GetPeliculasByIdCategoria(int id);
        ICollection<Pelicula> FindPeliculaByName(string nombre);
        bool Guardar();
    }
}
