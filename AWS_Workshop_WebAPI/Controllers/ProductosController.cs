using Microsoft.AspNetCore.Mvc;
using AWS_Workshop_Application.Services;

namespace AWS_Workshop_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductoService productoService, ILogger<ProductosController> logger)
        {
            _productoService = productoService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la lista de todos los productos (datos en memoria)
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<object>> ObtenerTodos()
        {
            try
            {
                _logger.LogInformation("GET /api/productos - Solicitando listado de todos los productos");
                var productos = _productoService.ObtenerTodos();
                _logger.LogInformation("GET /api/productos - Retornando {Count} productos", productos.Count());
                return Ok(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET /api/productos - Error al obtener listado de productos");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un producto específico por su ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<object> ObtenerPorId(int id)
        {
            try
            {
                _logger.LogInformation("GET /api/productos/{ProductoId} - Solicitando producto", id);
                var producto = _productoService.ObtenerPorId(id);
                _logger.LogInformation("GET /api/productos/{ProductoId} - Producto encontrado exitosamente", id);
                return Ok(producto);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("GET /api/productos/{ProductoId} - Producto no encontrado: {Message}", id, ex.Message);
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GET /api/productos/{ProductoId} - Error al obtener producto", id);
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }
    }
}
