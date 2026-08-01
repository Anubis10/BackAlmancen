using Microsoft.Extensions.Logging;

namespace BackAlmancen.Application.Features.Productos;

public class EliminarProductoHandler : IRequestHandler<EliminarProductoQuery, Response<bool>>
{
    private readonly IRepository<Producto> _repository;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<EliminarProductoHandler> _logger;

    public EliminarProductoHandler(IRepository<Producto> repository, IWebHostEnvironment env, ILogger<EliminarProductoHandler> logger)
    {
        _repository = repository;
        _env = env;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(EliminarProductoQuery request, CancellationToken cancellationToken)
    {
        var respuesta = new Response<bool>();

        var producto = await _repository.GetByIdAsync(request.Id);
        if (producto == null)
        {
            respuesta.Success = false;
            respuesta.MessageError = "El producto no existe.";
            return respuesta;
        }

        if (!string.IsNullOrEmpty(producto.ImageUrl))
        {
            var rutaRelativa = producto.ImageUrl.TrimStart('/', '\\');

            // Construir la ruta física completa en el servidor
            var rutaWebRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var rutaArchivoCompleta = Path.Combine(rutaWebRoot, rutaRelativa);

            // Verificar si el archivo existe en disco y eliminarlo
            if (File.Exists(rutaArchivoCompleta))
            {
                try
                {
                    File.Delete(rutaArchivoCompleta);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"No se pudo borrar el archivo físico: {ex.Message}");
                    
                }
            }
        }

        var eliminado = await _repository.DeleteAsync(request.Id);

        if (!eliminado)
        {
            respuesta.Success = false;
            respuesta.Data = false;
            respuesta.MessageError = $"No se encontró el producto con ID {request.Id} para eliminar.";
            return respuesta;
        }

        respuesta.Data = true;
        return respuesta;
    }
}
