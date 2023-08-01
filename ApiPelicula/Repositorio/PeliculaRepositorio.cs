using ApiPelicula.Data;
using ApiPelicula.Model;
using ApiPelicula.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPelicula.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public PeliculaRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _db.Peliculas.Update(pelicula);
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _db.Peliculas.Remove(pelicula);
            return Guardar();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _db.Peliculas.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(string nombre)
        {
            bool valor = _db.Peliculas.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExistePelicula(int id)
        {
            bool valor = _db.Peliculas.Any(c => c.Id == id);
            return valor;
        }

        

        public ICollection<Pelicula> GetPelicula()
        {
            return _db.Peliculas.OrderBy(c => c.Nombre).ToList();
        }

        public Pelicula GetPelicula(int id)
        {
            return _db.Peliculas.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Pelicula> GetPeliculasByIdCategoria(int id)
        {
            return _db.Peliculas.Include(ca=> ca.Categoria).Where(ca=>ca.CategoriaId==id).ToList();
        }
        public ICollection<Pelicula> FindPeliculaByName(string nombre)
        {
            IQueryable<Pelicula> query = _db.Peliculas;
            if (!string.IsNullOrEmpty(nombre))
            {
                query=query.Where(e=>e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
            }
            return query.ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true: false ;
        }
    }
}
