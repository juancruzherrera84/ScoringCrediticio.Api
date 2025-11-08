using Microsoft.AspNetCore.Mvc;
using Scoring.Api.Models;
using Scoring.Application.Interfaces;
using Scoring.Application.Services;
using Scoring.Domain;
using Scoring.Domain.Entities;
using Scoring.Domain.Enums;

namespace Scoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesController : ControllerBase
    {
        private readonly ISolicitudService _solicitudService;

        public SolicitudesController(ISolicitudService solicitudService)
        {
            _solicitudService = solicitudService;
        }

        // 🟦 1) Obtener TODAS las solicitudes
        // GET: api/solicitudes
        [HttpGet]
        public ActionResult<IEnumerable<SolicitudResponse>> ObtenerTodas()
        {
            var solicitudes = _solicitudService.ObtenerTodas();
            var lista = solicitudes.Select(MapToResponse).ToList();
            return Ok(lista);
        }

        // 🟦 2) Buscar con filtros (estado, riesgo, documento)
        // GET: api/solicitudes/buscar?estado=Aprobada&riesgo=Medio&documento=30123456
        [HttpGet("buscar")]
        public ActionResult<IEnumerable<SolicitudResponse>> Buscar(
            [FromQuery] EstadoSolicitud? estado,
            [FromQuery] NivelRiesgo? riesgo,
            [FromQuery] string documento)
        {
            var solicitudes = _solicitudService.Buscar(estado, riesgo, documento);
            var lista = solicitudes.Select(MapToResponse).ToList();
            return Ok(lista);
        }

        // 🟦 3) Cancelar una solicitud
        // PUT: api/solicitudes/{id}/cancelar
        [HttpPut("{id:int}/cancelar")]
        public ActionResult<SolicitudResponse> Cancelar(int id)
        {
            var solicitud = _solicitudService.CancelarSolicitud(id);
            if (solicitud == null)
                return NotFound();

            var response = MapToResponse(solicitud);
            return Ok(response);
        }

        // 🟦 4) Actualizar datos de una solicitud (monto / plazo) si está pendiente
        // PUT: api/solicitudes/{id}
        [HttpPut("{id:int}")]
        public ActionResult<SolicitudResponse> Actualizar(int id, [FromBody] ActualizarSolicitudRequest request)
        {
            var solicitud = _solicitudService.ActualizarSolicitud(
                id,
                request.MontoSolicitado,
                request.PlazoMeses
            );

            if (solicitud == null)
                return NotFound();

            var response = MapToResponse(solicitud);
            return Ok(response);
        }

        // 🟦 5) Ver SOLO el resultado de scoring
        // GET: api/solicitudes/{id}/scoring
        [HttpGet("{id:int}/scoring")]
        public ActionResult<object> ObtenerScoring(int id)
        {
            var resultado = _solicitudService.ObtenerResultadoScoring(id);
            if (resultado == null)
                return NotFound(new { Mensaje = "La solicitud no existe o aún no fue evaluada." });

            return Ok(new
            {
                resultado.Puntaje,
                resultado.NivelRiesgo,
                resultado.Observaciones,
                resultado.FechaCalculo
            });
        }

        // 🟦 6) (si querés) Listar por cliente
        // GET: api/solicitudes/cliente/1
        [HttpGet("cliente/{clienteId:int}")]
        public ActionResult<IEnumerable<SolicitudResponse>> ObtenerPorCliente(int clienteId)
        {
            var solicitudes = _solicitudService.ObtenerSolicitudesDeCliente(clienteId);
            var lista = solicitudes.Select(MapToResponse).ToList();
            return Ok(lista);
        }

        // 👇 Este MapToResponse ya lo debés tener, pero lo dejo por las dudas
        private SolicitudResponse MapToResponse(Solicitud solicitud)
        {
            return new SolicitudResponse
            {
                Id = solicitud.Id,
                ClienteNombreCompleto = solicitud.Cliente?.NombreCompleto,
                MontoSolicitado = solicitud.MontoSolicitado,
                PlazoMeses = solicitud.PlazoMeses,
                TipoProducto = solicitud.TipoProducto,
                Estado = solicitud.Estado,
                Puntaje = solicitud.ResultadoScoring?.Puntaje,
                NivelRiesgo = solicitud.ResultadoScoring?.NivelRiesgo,
                Observaciones = solicitud.ResultadoScoring?.Observaciones
            };
        }
    }
}
