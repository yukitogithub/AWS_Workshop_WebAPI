using AWS_Workshop_DataAccess.Models;
using Microsoft.Extensions.Logging;

namespace AWS_Workshop_Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly ILogger<ProductoService> _logger;
        
        private static readonly List<Producto> productos = new()
        {
            new Producto { Id = 1, Nombre = "Laptop", Descripcion = "Laptop de alto rendimiento", Precio = 1200.00m, Stock = 10 },
            new Producto { Id = 2, Nombre = "Mouse", Descripcion = "Mouse inalámbrico", Precio = 25.50m, Stock = 50 },
            new Producto { Id = 3, Nombre = "Teclado", Descripcion = "Teclado mecánico", Precio = 85.00m, Stock = 30 },
            new Producto { Id = 4, Nombre = "Monitor", Descripcion = "Monitor 4K", Precio = 450.00m, Stock = 15 },
            new Producto { Id = 5, Nombre = "Auriculares", Descripcion = "Auriculares con cancelación de ruido", Precio = 200.00m, Stock = 25 }
        };

        public ProductoService(ILogger<ProductoService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los productos (datos en memoria)");
                _logger.LogInformation("Se obtuvieron {Count} productos exitosamente", productos.Count);
                return productos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los productos");
                throw;
            }
        }

        public Producto ObtenerPorId(int id)
        {
            try
            {
                _logger.LogInformation("Buscando producto con ID: {ProductoId}", id);
                var producto = productos.FirstOrDefault(p => p.Id == id);
                
                if (producto == null)
                {
                    _logger.LogWarning("Producto con ID {ProductoId} no encontrado", id);
                    throw new KeyNotFoundException($"Producto con ID {id} no encontrado");
                }
                
                _logger.LogInformation("Producto con ID {ProductoId} encontrado: {ProductoNombre}", id, producto.Nombre);
                return producto;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto con ID: {ProductoId}", id);
                throw;
            }
        }
    }
}
