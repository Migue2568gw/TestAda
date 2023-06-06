using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using Tesy_ADA.Datos;
using Tesy_ADA.Models;

namespace Tesy_ADA.Controllers
{
    public class LoginController : ApiController
    {
        private readonly CarritoComprasContext _dbContext;

        public LoginController()
        {
            _dbContext = new CarritoComprasContext();
        }

        [HttpPost]
        public IHttpActionResult Login(LoginModel loginModel)
        {
            try
            {
                List<Usuario> usuarios = _dbContext.ConsultarUsuarios();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (usuarios.Count == 0)
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, new { Codigo = (int)HttpStatusCode.OK, Mensaje = "No hay usuarios creados" });
                    return ResponseMessage(response);
                }

                var usuarioEncontrado = usuarios.FirstOrDefault(u => u.UsuarioLog == loginModel.Usuario);

                if (usuarioEncontrado != null)
                {
                    if (usuarioEncontrado.Contrasena == loginModel.Contrasena)
                    {
                        var response = Request.CreateResponse(HttpStatusCode.OK, new { Codigo = (int)HttpStatusCode.OK, UsuarioId = usuarioEncontrado.UsuarioId });
                        return ResponseMessage(response);
                    }
                    else
                    {
                        var response = Request.CreateResponse(HttpStatusCode.OK, new { Codigo = (int)HttpStatusCode.OK, Mensaje = "Contraseña incorrecta" });
                        return ResponseMessage(response);
                    }
                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, new { Codigo = (int)HttpStatusCode.OK, Mensaje = "Usuario no existente, por favor, cree un nuevo usuario y contraseña." }); 
                    return ResponseMessage(response);
                }
            }
            catch (System.Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                return ResponseMessage(response);
            }
        }

    }
}
