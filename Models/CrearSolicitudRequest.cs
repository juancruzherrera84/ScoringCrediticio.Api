using Scoring.Domain.Enums;

namespace Scoring.Api.Models
{
    public class CrearSolicitudRequest
    {
        // Datos del cliente
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public decimal IngresosMensuales { get; set; }

        // Datos de la solicitud
        public TipoProducto TipoProducto { get; set; }
        public decimal MontoSolicitado { get; set; }
        public int PlazoMeses { get; set; }
    }
}
