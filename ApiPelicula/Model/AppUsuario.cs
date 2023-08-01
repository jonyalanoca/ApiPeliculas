using Microsoft.AspNetCore.Identity;

namespace ApiPelicula.Model
{
    public class AppUsuario:IdentityUser
    {
        //añadir campos personalizados
        public string Nombre { get; set; }
    }
}
