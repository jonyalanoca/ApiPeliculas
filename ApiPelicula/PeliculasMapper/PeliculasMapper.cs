using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;
using AutoMapper;

namespace ApiPelicula.PeliculasMapper
{
    public class PeliculasMapper:Profile
    {
        public PeliculasMapper()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();  
            CreateMap<Pelicula, PeliculaDto>().ReverseMap();
            
        }
    }
}
