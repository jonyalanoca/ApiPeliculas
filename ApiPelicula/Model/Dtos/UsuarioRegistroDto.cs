using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Model.Dtos
{
    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage ="El nombre de usuario es requerido")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La clave es requerida")]
        public string Clave { get; set; }
        public string Rol { get; set; }
    }
}
