using Scoring.Domain.Enums;

namespace Scoring.Api.Models
{
    public class SolicitudResponse
    {
        public int Id { get; set; }
        public string ClienteNombreCompleto { get; set; }
        public decimal MontoSolicitado { get; set; }
        public int PlazoMeses { get; set; }
        public TipoProducto TipoProducto { get; set; }
        public EstadoSolicitud Estado { get; set; }
        public double? Puntaje { get; set; }
        public NivelRiesgo? NivelRiesgo { get; set; }
        public string Observaciones { get; set; }
    }
}
