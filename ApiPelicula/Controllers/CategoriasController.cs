using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPelicula.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/categorias")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRep;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepositorio ctRep, IMapper mapper)
        {
            _ctRep = ctRep;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        [ResponseCache(Duration = 20)] //durante 20 segundos devolvera la misma consulta(se guarda en la cache) para consultas que devulven la misma data
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias() {
            var listaCategorias = _ctRep.GetCategoria();
            var listaCategoriasDto = new List<CategoriaDto>();
            foreach(var lista  in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(lista));
            }
            return Ok(listaCategoriasDto);
                
        }
        [AllowAnonymous]
        [HttpGet("{categoriaId:int}",Name ="GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCategorias(int categoriaId)
        {
            var itemCategoria = _ctRep.GetCategoria(categoriaId);
            if (itemCategoria == null)
            {
                return NotFound();
            }
            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);
            return Ok(itemCategoriaDto);

        }
        [Authorize(Roles ="admin")]
        [HttpPost]
        [ProducesResponseType(201, Type=typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult  CrearCategoria([FromBody]CrearCategoriaDto crearCategoriaDto)
        {   
            //validaciones
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (crearCategoriaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_ctRep.ExisteCategoria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }
            //codigo
            var categoria=_mapper.Map<Categoria>(crearCategoriaDto);
            if (!_ctRep.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal en el guardado de registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategoria",new {categoriaId=categoria.Id},categoria);
        }
        [Authorize(Roles = "admin")]
        [HttpPatch("{categoriaId:int}",Name ="ActualizarPatchCategoria")]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarPatchCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            //validaciones
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoriaDto == null || categoriaId!= categoriaDto.Id)
            {
                return BadRequest(ModelState);
            }
            
            //codigo
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (!_ctRep.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal en el actualizando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BorrarCategoria(int categoriaId)
        {
            //validaciones
            if (!_ctRep.ExisteCategoria(categoriaId))
            {
                return NotFound();
            }

            //codigo
            var categoria = _ctRep.GetCategoria(categoriaId);
            if (!_ctRep.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal en el borrandos el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
