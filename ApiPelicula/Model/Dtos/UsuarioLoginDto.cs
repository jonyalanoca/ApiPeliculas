using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Model.Dtos
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage ="El nombre de usuario es requerido")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La clave es requerida")]
        public string Clave { get; set; }
    }
}
