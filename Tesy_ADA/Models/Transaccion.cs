using System;

namespace Tesy_ADA.Models
{
    public class Transaccion
    {
        public int TransaccionId { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaTransaccion { get; set; }
    }
}