using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Util;
using Tesy_ADA.Datos;
using Tesy_ADA.Models;

namespace Tesy_ADA.Controllers
{
    public class CarritoComprasController : ApiController
    {
        private readonly CarritoComprasContext _dbContext;

        public CarritoComprasController()
        {
            _dbContext = new CarritoComprasContext();
        }

        [HttpPost]
        [Route("api/adicionarUsuario")]
        public IHttpActionResult AdicionarUsuario(Usuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //Asignar valor de usuario 
                if (usuario.Rol == 0)
                    usuario.Rol = 2;

                var insert = _dbContext.CrearUsuarios(usuario);

                if (insert == null)
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, new { Codigo = (int)HttpStatusCode.OK, Mensaje = "Error al Adicionar Usuario" });
                    return ResponseMessage(response);
                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, new { Codigo = (int)HttpStatusCode.OK, Mensaje = insert });
                    return ResponseMessage(response);
                }
            }
            catch (System.Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.OK, new { Codigo = (int)HttpStatusCode.OK, Mensaje = e.Message });
                return ResponseMessage(response);
            }
        }

        [HttpGet]
        [Route("api/gettransacciones")]
        public IHttpActionResult GetTransacciones()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                List<Transaccion> transaccions = new List<Transaccion>();
                transaccions = _dbContext.ConsultarTrasaccion();

                return Ok(transaccions);
            }
            catch (System.Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                return ResponseMessage(response);
            }
        }

        [HttpGet]
        [Route("api/getusuarios")]
        public IHttpActionResult GetUsuarios()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                List<Usuario> usuarios = new List<Usuario>();
                usuarios = _dbContext.ConsultarUsuarios();

                return Ok(usuarios);
            }
            catch (System.Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                return ResponseMessage(response);
            }
        }

        [HttpGet]
        [Route("api/getproducto")]
        public IHttpActionResult GetProducto()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                List<Producto> productos = new List<Producto>();
                productos = _dbContext.ConsultarProductos();

                return Ok(productos);
            }
            catch (System.Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                return ResponseMessage(response);
            }
        }

        [HttpPost]
        [Route("api/getproductoId/{idproducto}")]
        public IHttpActionResult GetProductoId(int idproducto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                List<Producto> productos = new List<Producto>();
                productos = _dbContext.ConsultarProductos();
                Producto pruducto = new Producto();
                if (productos.Count > 0)
                {
                    pruducto = productos.FirstOrDefault(u => u.ProductoId == idproducto);
                }
                return Ok(pruducto);
            }
            catch (System.Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                return ResponseMessage(response);
            }
        }

        [HttpPost]
        [Route("api/adicionarproducto")]
        public IHttpActionResult AdicionarPproducto(Producto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var insert = _dbContext.CrearProducto(producto);

                if (insert == null)
                {
                    var response = Request.CreateResponse(HttpStatusCode.Continue, "Error al Adicionar producto");
                    return ResponseMessage(response);
                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, insert);
                    return ResponseMessage(response);
                }
            }
            catch (System.Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                return ResponseMessage(response);
            }
        }

        [HttpPost]
        [Route("api/modificarproducto")]
        public IHttpActionResult ModificarPproducto(Producto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var insert = _dbContext.ModificarProducto(producto);

                if (insert == null)
                {
                    var response = Request.CreateResponse(HttpStatusCode.Continue, "Error al Modificar producto");
                    return ResponseMessage(response);
                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, insert);
                    return ResponseMessage(response);
                }
            }
            catch (System.Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                return ResponseMessage(response);
            }
        }
        [HttpPost]
        [Route("api/generartransaccion")]
        public IHttpActionResult GenerarTransasccion(List<Transaccion> transacciones)
        {
            try
            {
                string mensaje = string.Empty;
                if (transacciones != null)
                {
                    if (transacciones.Count > 0)
                    {
                        int cantIni = transacciones.Where(t => t.Cantidad > 0).Count();
                        int cantFin = 0;
                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }
                        foreach (var item in transacciones)
                        {
                            if (item.Cantidad != 0)
                            {
                                var insert = _dbContext.ActualizarProducto(item.ProductoId, item.Cantidad, item.UsuarioId);

                                if (insert != null)
                                    cantFin = cantFin + 1;
                            }
                        }

                        mensaje = "De " + cantIni + " productos, fueron exitosos " + cantFin;
                        var response = Request.CreateResponse(HttpStatusCode.OK, new { Codigo = (int)HttpStatusCode.OK, Mensaje = mensaje });
                        return ResponseMessage(response);
                    }
                    else
                    {
                        var response = Request.CreateResponse(HttpStatusCode.OK, new { Codigo = (int)HttpStatusCode.OK, Mensaje = "Sin pedidos" });
                        return ResponseMessage(response);
                    }

                }
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, new { Codigo = (int)HttpStatusCode.OK, Mensaje = "Sin pedidos" });
                    return ResponseMessage(response);
                }

            }
            catch (System.Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.OK, new { Codigo = (int)HttpStatusCode.OK, Mensaje = e.Message });
                return ResponseMessage(response);
            }
        }
    }
}
