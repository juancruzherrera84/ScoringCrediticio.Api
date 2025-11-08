namespace Scoring.Api.Models
{
    public class ClienteRequest
    {
        // Datos básicos
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public long Documento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        // Datos financieros
        public decimal IngresosMensuales { get; set; }
        public bool TieneCuentaBancaria { get; set; }
        public bool TieneTarjetaCredito { get; set; }
        public int AntiguedadLaboralMeses { get; set; }
        public bool TieneDeudasRegistradas { get; set; }
        public decimal MontoDeudaAproximado { get; set; }
        public int CantidadTarjetasCredito { get; set; }
    }
}
