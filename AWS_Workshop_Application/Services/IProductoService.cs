using AWS_Workshop_DataAccess.Models;

namespace AWS_Workshop_Application.Services
{
    public interface IProductoService
    {
        IEnumerable<Producto> ObtenerTodos();
        Producto ObtenerPorId(int id);
    }
}
