﻿using ApiPelicula.Data;
using ApiPelicula.Model;
using ApiPelicula.Repositorio.IRepositorio;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPelicula.Repositorio
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _db.Categorias.Update(categoria);
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _db.Categorias.Remove(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _db.Categorias.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
            bool valor = _db.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public bool ExisteCategoria(int id)
        {
            bool valor = _db.Categorias.Any(c => c.Id == id);
            return valor;
        }

        public ICollection<Categoria> GetCategoria()
        {
            return _db.Categorias.OrderBy(c => c.Nombre).ToList();
        }

        public Categoria GetCategoria(int id)
        {
            return _db.Categorias.FirstOrDefault(c => c.Id == id);
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true: false ;
        }
    }
}
