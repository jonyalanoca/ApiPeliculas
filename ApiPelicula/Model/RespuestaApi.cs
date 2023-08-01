using System.Net;

namespace ApiPelicula.Model
{
    public class RespuestaApi
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
        public RespuestaApi()
        {
            ErrorMessages = new List<string>();
        }
        
    }
    
}
