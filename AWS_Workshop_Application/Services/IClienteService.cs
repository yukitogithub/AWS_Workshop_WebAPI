using AWS_Workshop_DataAccess.Models;

namespace AWS_Workshop_Application.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();
        Task<Cliente> ObtenerPorIdAsync(int id);
        Task<Cliente> CrearAsync(Cliente cliente);
        Task<Cliente> ActualizarAsync(Cliente cliente);
        Task EliminarAsync(int id);
    }
}
