using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Model
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NombreUsuario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Clave { get; set; }
        [Required]
        public string Rol { get; set; }
    }
}
