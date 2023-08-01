using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Model.Dtos
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre es obligatorio")]
        [MaxLength(250, ErrorMessage ="El maximo de caracteres es de 250")]
        public string Nombre { get; set; }
    }
}
