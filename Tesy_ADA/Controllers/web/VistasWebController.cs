using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesy_ADA.Datos;
using Tesy_ADA.Models;

namespace Tesy_ADA.Controllers.web
{
    public class VistasWebController : Controller
    {
        private readonly CarritoComprasContext _dbContext;

        public VistasWebController()
        {
            _dbContext = new CarritoComprasContext();
        }
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult AdicionarUsuario()
        {
            return View();
        }
        public ActionResult ResultUsuarios(int usuarioId)
        {
            List<Usuario> usuarios = _dbContext.ConsultarUsuarios();
            var usuarioEncontrado = usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);
            Session["usuarioEncontrado"] = usuarioEncontrado;

            if (usuarioEncontrado.Rol == 1)
            {
                return View("Transacciones");
            }
            else if (usuarioEncontrado.Rol == 2)
            {
                return View("IndexCliente", usuarioEncontrado.UsuarioId);
            }

            return View();
        }
        public ActionResult IndexCliente(Usuario usuario)
        {
            if (Session["usuarioEncontrado"] == null)
            {
                return View("Login");
            }
            else if ((Session["usuarioEncontrado"] as Usuario)?.Rol == 1)
            {
                return View("Transacciones");
            }
            return View(usuario);
        }

        public ActionResult Transacciones()
        {
            if (Session["usuarioEncontrado"] == null)
            {
                return View("Login");
            }
            else if ((Session["usuarioEncontrado"] as Usuario)?.Rol == 2)
            {
                return View("IndexCliente", (Session["usuarioEncontrado"] as Usuario)?.UsuarioId);
            }
            return View();
        }
        public ActionResult Usuarios()
        {
            if (Session["usuarioEncontrado"] == null)
            {
                return View("Login");
            }
            else if ((Session["usuarioEncontrado"] as Usuario)?.Rol == 2)
            {
                return View("IndexCliente", (Session["usuarioEncontrado"] as Usuario)?.UsuarioId);
            }
            return View();
        }
        public ActionResult Productos()
        {
            if (Session["usuarioEncontrado"] == null)
            {
                return View("Login");
            }
            else if ((Session["usuarioEncontrado"] as Usuario)?.Rol == 2)
            {
                return View("IndexCliente", (Session["usuarioEncontrado"] as Usuario)?.UsuarioId);
            }
            return View();
        }
        public ActionResult ModificarProducto()
        {
            if (Session["usuarioEncontrado"] == null)
            {
                return View("Login");
            }
            else if ((Session["usuarioEncontrado"] as Usuario)?.Rol == 2)
            {
                return View("IndexCliente", (Session["usuarioEncontrado"] as Usuario)?.UsuarioId);
            }
            return View();
        }
        public ActionResult AdicionarProducto()
        {
            if (Session["usuarioEncontrado"] == null)
            {
                return View("Login");
            }
            else if ((Session["usuarioEncontrado"] as Usuario)?.Rol == 2)
            {
                return View("IndexCliente", (Session["usuarioEncontrado"] as Usuario)?.UsuarioId);
            }
            return View();
        }
        public ActionResult CerrarSe()
        {
            Session["usuarioEncontrado"] = null;
            return Json(true);
        }
    }
}