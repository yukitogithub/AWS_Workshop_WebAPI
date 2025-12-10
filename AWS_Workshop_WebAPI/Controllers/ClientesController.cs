using Microsoft.AspNetCore.Mvc;
using AWS_Workshop_Application.Services;
using AWS_Workshop_DataAccess.Models;
using Microsoft.Extensions.Logging;

namespace AWS_Workshop_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(IClienteService clienteService, ILogger<ClientesController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la lista de todos los clientes desde PostgreSQL
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> ObtenerTodos()
        {
            try
            {
                _logger.LogInformation("GET /api/clientes - Solicitando listado de todos los clientes");
                var clientes = await _clienteService.ObtenerTodosAsync();
                _logger.LogInformation("GET /api/clientes - Retornando {Count} clientes", (clientes as IEnumerable<Cliente>)?.Count() ?? 0);
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET /api/clientes - Error al obtener listado de clientes");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un cliente específico por su ID desde PostgreSQL
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> ObtenerPorId(int id)
        {
            try
            {
                _logger.LogInformation("GET /api/clientes/{ClienteId} - Solicitando cliente", id);
                var cliente = await _clienteService.ObtenerPorIdAsync(id);
                _logger.LogInformation("GET /api/clientes/{ClienteId} - Cliente encontrado exitosamente", id);
                return Ok(cliente);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("GET /api/clientes/{ClienteId} - Cliente no encontrado: {Message}", id, ex.Message);
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET /api/clientes/{ClienteId} - Error al obtener cliente", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea un nuevo cliente en PostgreSQL
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Cliente>> Crear([FromBody] Cliente cliente)
        {
            _logger.LogInformation("POST /api/clientes - Solicitando creación de cliente: {ClienteNombre}", cliente?.Nombre);
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("POST /api/clientes - Validación de modelo falló");
                return BadRequest(ModelState);
            }

            try
            {
                var clienteCreado = await _clienteService.CrearAsync(cliente);
                _logger.LogInformation("POST /api/clientes - Cliente creado exitosamente con ID: {ClienteId}", clienteCreado.Id);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = clienteCreado.Id }, clienteCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST /api/clientes - Error al crear cliente: {ClienteNombre}", cliente?.Nombre);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un cliente existente en PostgreSQL
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Cliente>> Actualizar(int id, [FromBody] Cliente cliente)
        {
            _logger.LogInformation("PUT /api/clientes/{ClienteId} - Solicitando actualización de cliente", id);
            
            if (id != cliente.Id)
            {
                _logger.LogWarning("PUT /api/clientes/{ClienteId} - El ID de la URL no coincide con el ID del cliente: {ClienteBodyId}", id, cliente.Id);
                return BadRequest(new { mensaje = "El ID en la URL no coincide con el ID del cliente" });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("PUT /api/clientes/{ClienteId} - Validación de modelo falló", id);
                return BadRequest(ModelState);
            }

            try
            {
                var clienteActualizado = await _clienteService.ActualizarAsync(cliente);
                _logger.LogInformation("PUT /api/clientes/{ClienteId} - Cliente actualizado exitosamente", id);
                return Ok(clienteActualizado);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("PUT /api/clientes/{ClienteId} - Cliente no encontrado: {Message}", id, ex.Message);
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PUT /api/clientes/{ClienteId} - Error al actualizar cliente", id);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un cliente de PostgreSQL
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                _logger.LogInformation("DELETE /api/clientes/{ClienteId} - Solicitando eliminación de cliente", id);
                await _clienteService.EliminarAsync(id);
                _logger.LogInformation("DELETE /api/clientes/{ClienteId} - Cliente eliminado exitosamente", id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("DELETE /api/clientes/{ClienteId} - Cliente no encontrado: {Message}", id, ex.Message);
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DELETE /api/clientes/{ClienteId} - Error al eliminar cliente", id);
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
