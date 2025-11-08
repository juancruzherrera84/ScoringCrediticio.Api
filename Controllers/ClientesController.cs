using Microsoft.AspNetCore.Mvc;
using Scoring.Api.Models;
using Scoring.Application.Interfaces;
using Scoring.Domain;
using Scoring.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Scoring.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // POST: api/clientes
        [HttpPost]
        public ActionResult<ClienteResponse> Crear([FromBody] ClienteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = MapToEntity(request);
            var creado = _clienteService.Crear(cliente);

            var response = MapToResponse(creado);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, response);
        }

        // GET: api/clientes
        [HttpGet]
        public ActionResult<IEnumerable<ClienteResponse>> ObtenerTodos()
        {
            var clientes = _clienteService.ObtenerTodos();
            var lista = clientes.Select(MapToResponse).ToList();
            return Ok(lista);
        }

        // GET: api/clientes/{id}
        [HttpGet("{id:int}")]
        public ActionResult<ClienteResponse> ObtenerPorId(int id)
        {
            var cliente = _clienteService.ObtenerPorId(id);
            if (cliente == null)
                return NotFound();

            return Ok(MapToResponse(cliente));
        }

        // GET: api/clientes/documento/30123456
        [HttpGet("documento/{documento}")]
        public ActionResult<ClienteResponse> ObtenerPorDocumento(long documento)
        {
            var cliente = _clienteService.ObtenerPorDocumento(documento);
            if (cliente == null)
                return NotFound();

            return Ok(MapToResponse(cliente));
        }

        // PUT: api/clientes/{id}
        [HttpPut("{id:int}")]
        public ActionResult<ClienteResponse> Actualizar(int id, [FromBody] ClienteRequest request)
        {
            var entidad = MapToEntity(request);
            entidad.Id = id;

            var actualizado = _clienteService.Actualizar(entidad);
            if (actualizado == null)
                return NotFound();

            return Ok(MapToResponse(actualizado));
        }

        // 🔁 Mapeos

        private Cliente MapToEntity(ClienteRequest request)
        {
            return new Cliente
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Documento = request.Documento,
                Email = request.Email,
                Telefono = request.Telefono,
                DatosFinancieros = new DatosFinancieros
                {
                    IngresosMensuales = request.IngresosMensuales,
                    TieneCuentaBancaria = request.TieneCuentaBancaria,
                    TieneTarjetaCredito = request.TieneTarjetaCredito,
                    AntiguedadLaboralMeses = request.AntiguedadLaboralMeses,
                    TieneDeudasRegistradas = request.TieneDeudasRegistradas,
                    MontoDeudaAproximado = request.MontoDeudaAproximado,
                    CantidadTarjetasCredito = request.CantidadTarjetasCredito
                }
            };
        }

        private ClienteResponse MapToResponse(Cliente cliente)
        {
            return new ClienteResponse
            {
                Id = cliente.Id,
                NombreCompleto = cliente.NombreCompleto,
                Documento = cliente.Documento,
                Email = cliente.Email,
                Telefono = cliente.Telefono,
                IngresosMensuales = cliente.DatosFinancieros?.IngresosMensuales ?? 0,
                TieneCuentaBancaria = cliente.DatosFinancieros?.TieneCuentaBancaria ?? false,
                TieneTarjetaCredito = cliente.DatosFinancieros?.TieneTarjetaCredito ?? false,
                AntiguedadLaboralMeses = cliente.DatosFinancieros?.AntiguedadLaboralMeses ?? 0,
                TieneDeudasRegistradas = cliente.DatosFinancieros?.TieneDeudasRegistradas ?? false,
                MontoDeudaAproximado = cliente.DatosFinancieros?.MontoDeudaAproximado ?? 0,
                CantidadTarjetasCredito = cliente.DatosFinancieros?.CantidadTarjetasCredito ?? 0
            };
        }
    }
}
