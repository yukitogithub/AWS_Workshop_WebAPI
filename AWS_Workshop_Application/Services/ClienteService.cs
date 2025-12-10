using AWS_Workshop_DataAccess.Context;
using AWS_Workshop_DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AWS_Workshop_Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(ApplicationDbContext context, ILogger<ClienteService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los clientes de la base de datos");
                var clientes = await _context.Clientes.ToListAsync();
                _logger.LogInformation("Se obtuvieron {Count} clientes exitosamente", clientes.Count);
                return clientes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los clientes");
                throw;
            }
        }

        public async Task<Cliente> ObtenerPorIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Buscando cliente con ID: {ClienteId}", id);
                var cliente = await _context.Clientes.FindAsync(id);
                
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente con ID {ClienteId} no encontrado", id);
                    throw new KeyNotFoundException($"Cliente con ID {id} no encontrado");
                }
                
                _logger.LogInformation("Cliente con ID {ClienteId} encontrado: {ClienteNombre}", id, cliente.Nombre);
                return cliente;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente con ID: {ClienteId}", id);
                throw;
            }
        }

        public async Task<Cliente> CrearAsync(Cliente cliente)
        {
            try
            {
                _logger.LogInformation("Creando nuevo cliente: {ClienteNombre}, Email: {ClienteEmail}", cliente.Nombre, cliente.Email);
                cliente.FechaRegistro = DateTime.Now;
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cliente creado exitosamente con ID: {ClienteId}", cliente.Id);
                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente: {ClienteNombre}", cliente.Nombre);
                throw;
            }
        }

        public async Task<Cliente> ActualizarAsync(Cliente cliente)
        {
            try
            {
                _logger.LogInformation("Actualizando cliente con ID: {ClienteId}", cliente.Id);
                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cliente con ID {ClienteId} actualizado exitosamente", cliente.Id);
                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente con ID: {ClienteId}", cliente.Id);
                throw;
            }
        }

        public async Task EliminarAsync(int id)
        {
            try
            {
                _logger.LogInformation("Intentando eliminar cliente con ID: {ClienteId}", id);
                var cliente = await _context.Clientes.FindAsync(id);
                
                if (cliente == null)
                {
                    _logger.LogWarning("No se puede eliminar. Cliente con ID {ClienteId} no encontrado", id);
                    throw new KeyNotFoundException($"Cliente con ID {id} no encontrado");
                }
                
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cliente con ID {ClienteId} eliminado exitosamente", id);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cliente con ID: {ClienteId}", id);
                throw;
            }
        }
    }
}
