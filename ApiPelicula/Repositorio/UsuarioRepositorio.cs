using ApiPelicula.Data;
using ApiPelicula.Model;
using ApiPelicula.Model.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlTypes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiPelicula.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _db;
        private string claveSecreta;

        public UsuarioRepositorio(ApplicationDbContext db,IConfiguration config)
        {
            _db = db;
            this.claveSecreta = config.GetValue<string>("ApiSettings:Secreta");

        }
        public Usuario GetUsuario(int id)
        {
            return _db.Usuarios.FirstOrDefault(u =>u.Id == id);
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _db.Usuarios.OrderBy(u => u.NombreUsuario).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            var usuarioEncontrado= _db.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuario);
            if (usuarioEncontrado == null)
            {
                return true;
            }
            return false;

        }
        
        
        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            var passwordEncriptado = toSha256(usuarioRegistroDto.Clave);
            Usuario usuario = new Usuario()
            {
                NombreUsuario = usuarioRegistroDto.NombreUsuario,
                Clave = usuarioRegistroDto.Clave,
                Nombre = usuarioRegistroDto.Nombre,
                Rol = usuarioRegistroDto.Rol
            };
            usuario.Clave = passwordEncriptado.ToString();
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();
           
            return usuario;
        }
        public async Task<UsuarioLoginRespuestaDto> login(UsuarioLoginDto usuaioLoginDto)
        {
            var passwordEncriptado = toSha256(usuaioLoginDto.Clave);
            var usuario = _db.Usuarios.FirstOrDefault(u=>u.NombreUsuario.ToLower()== usuaioLoginDto.NombreUsuario.ToLower() && u.Clave==passwordEncriptado);
            if(usuario == null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }
            var manejadorToken = new JwtSecurityTokenHandler();
            var llave = Encoding.ASCII.GetBytes(claveSecreta);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Rol),

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(llave),SecurityAlgorithms.HmacSha256Signature)
            };
            var token=manejadorToken.CreateToken(tokenDescriptor);
            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = usuario
            };
            return usuarioLoginRespuestaDto;

        }
        public static string toSha256(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[]data =System.Text.Encoding.UTF8.GetBytes(valor);
            data=x.ComputeHash(data);
            string resp = "";
            for(int i=0;i<data.Length;i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;
           
        }
    }
}
