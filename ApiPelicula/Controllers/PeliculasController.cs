using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPelicula.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _pelRep;
        private readonly IMapper _mapper;

        public PeliculasController(IPeliculaRepositorio pelRep, IMapper mapper)
        {
            _pelRep = pelRep;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _pelRep.GetPelicula();
            var listaPeliculasDto = new List<PeliculaDto>();
            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(lista));
            }
            return Ok(listaPeliculasDto);

        }
        [AllowAnonymous]
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _pelRep.GetPelicula(peliculaId);
            if (itemPelicula == null)
            {
                return NotFound();
            }
            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);
            return Ok(itemPeliculaDto);

        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPelicula([FromBody] PeliculaDto peliculaDto)
        {
            //validaciones
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (peliculaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_pelRep.ExistePelicula(peliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La pelicula ya existe");
                return StatusCode(404, ModelState);
            }
            //codigo
            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            if (!_pelRep.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal en el guardado de registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }
        [Authorize(Roles = "admin")]
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            //validaciones
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //codigo
            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            if (!_pelRep.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal en el actualizando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BorrarPelicula(int peliculaId)
        {
            //validaciones
            if (!_pelRep.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            //codigo
            var pelicula = _pelRep.GetPelicula(peliculaId);
            if (!_pelRep.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal en el borrandos el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [AllowAnonymous]
        [HttpGet("GetPeliculaEnCategoria/{categoriaId:int}")]
        public IActionResult GetPeliculaPorIdCategoria(int categoriaId) 
        {
            var listaPeliculas= _pelRep.GetPeliculasByIdCategoria(categoriaId);
            if (listaPeliculas == null)
            {
                return NotFound();
            }
            var itemPelicula=new List<PeliculaDto>();
            foreach(var item in listaPeliculas)
            {
                itemPelicula.Add(_mapper.Map<PeliculaDto>(item));
            }
            return Ok(itemPelicula);

        }
        [AllowAnonymous]
        [HttpGet("GetPeliculaEnNombre/{nombrePelicula}")]
        public IActionResult GetPeliculaPorNombre(string nombrePelicula)
        {

            try
            {
                var resultado = _pelRep.FindPeliculaByName(nombrePelicula.Trim());
                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound();
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }

        }
    }
}
