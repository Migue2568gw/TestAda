using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Tesy_ADA.Models;

namespace Tesy_ADA.Datos
{
    public class CarritoComprasContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        public CarritoComprasContext() : base("AdaTestBdConexion")
        {
        }

        public string ActualizarProducto(int productoId, int cantidad, int usuarioId)
        {
            try
            {
                string resultado = string.Empty;
                var parametros = new[]
                 {
                    new SqlParameter("ProductoId", SqlDbType.Int) { Value = productoId },
                    new SqlParameter("Cantidad", SqlDbType.Int) { Value = cantidad },
                    new SqlParameter("UsuarioId", SqlDbType.Int){ Value = usuarioId}
                };

                var exec = Database.ExecuteSqlCommand("EXEC SP_ActualizarProducto @ProductoId, @Cantidad, @UsuarioId", parametros);

                if (exec > 0)
                {
                    resultado = "Pedido realizado correctamente";
                }
                return resultado;
            }
            catch (Exception e)
            {

                throw new InvalidCastException(e.Message);
            }
        }

        public List<Usuario> ConsultarUsuarios()
        {
            try
            {
                var User = Database.SqlQuery<Usuario>("SP_ConsultarUsuarios").ToList();
                return User;
            }
            catch (Exception e)
            {
                throw new InvalidCastException(e.Message);
            }
        }

        public string CrearUsuarios(Usuario usuario)
        {
            try
            {
                string resultado = string.Empty;
                var parametros = new[]
                 {
                    new SqlParameter("Nombres", SqlDbType.NVarChar) { Value = usuario.Nombres, Size = 50 },
                    new SqlParameter("Direccion", SqlDbType.NVarChar) { Value = usuario.Direccion, Size = 100 },
                    new SqlParameter("Telefono", usuario.Telefono),
                    new SqlParameter("Identificacion", SqlDbType.NVarChar) { Value = usuario.Identificacion, Size = 20 },
                    new SqlParameter("UsuarioLog", SqlDbType.NVarChar) { Value = usuario.UsuarioLog, Size = 50 },
                    new SqlParameter("Contrasena", SqlDbType.NVarChar) { Value = usuario.Contrasena, Size = 50 },
                    new SqlParameter("Rol", usuario.Rol)
                };

                var exec = Database.ExecuteSqlCommand("EXEC SP_CrearUsuarios @Nombres, @Direccion, @Telefono, @Identificacion, @UsuarioLog, @Contrasena, @Rol", parametros);

                if (exec > 0 )
                {
                    resultado = "Usuario Creado Correctamente";
                }
                return resultado;
            }
            catch (Exception e)
            {
                throw new InvalidCastException(e.Message);
            }
        }

        public List<Transaccion> ConsultarTrasaccion()
        {
            try
            {
                var transaccions = Database.SqlQuery<Transaccion>("SP_GetTrasaccion").ToList();
                return transaccions;
            }
            catch (Exception e)
            {
                throw new InvalidCastException(e.Message);
            }
        }

        public List<Producto> ConsultarProductos()
        {
            try
            {
                var productos = Database.SqlQuery<Producto>("SP_GetProducto").ToList();
                return productos;
            }
            catch (Exception e)
            {
                throw new InvalidCastException(e.Message);
            }
        }
        
        public string CrearProducto(Producto producto)
        {
            try
            {
                string resultado = string.Empty;
                var parametros = new[]
                 {
                   new SqlParameter("ProductoId", SqlDbType.NVarChar) { Value = DBNull.Value  },
                    new SqlParameter("Nombre", SqlDbType.NVarChar) { Value = producto.Nombre },
                    new SqlParameter("CantidadDisponible", SqlDbType.Int) { Value = producto.CantidadDisponible },
                    new SqlParameter("Descripcion",SqlDbType.NVarChar) { Value = producto.Descripcion},
                    new SqlParameter("OPC", SqlDbType.NVarChar) { Value = "CREA"},
               
                };

                var exec = Database.ExecuteSqlCommand("EXEC SP_CRUDPROD @ProductoId, @Nombre, @CantidadDisponible, @Descripcion, @OPC", parametros);

                if (exec > 0)
                {
                    resultado = "Producto Creado Correctamente";
                }
                return resultado;
            }
            catch (Exception e)
            {
                throw new InvalidCastException(e.Message);
            }
        }

        public string ModificarProducto(Producto producto)
        {
            try
            {
                string resultado = string.Empty;
                var parametros = new[]
                 {
                    new SqlParameter("ProductoId", SqlDbType.NVarChar) { Value = producto.ProductoId },
                    new SqlParameter("Nombre", SqlDbType.NVarChar) { Value = producto.Nombre },
                    new SqlParameter("CantidadDisponible", SqlDbType.Int) { Value = producto.CantidadDisponible },
                    new SqlParameter("Descripcion",SqlDbType.NVarChar) { Value = producto.Descripcion},
                    new SqlParameter("OPC", SqlDbType.NVarChar) { Value = "MODI"},

                };

                var exec = Database.ExecuteSqlCommand("EXEC SP_CRUDPROD @ProductoId, @Nombre, @CantidadDisponible, @Descripcion, @OPC", parametros);

                if (exec > 0)
                {
                    resultado = "Producto Modificado Correctamente";
                }
                return resultado;
            }
            catch (Exception e)
            {
                throw new InvalidCastException(e.Message);
            }
        }
    }
}