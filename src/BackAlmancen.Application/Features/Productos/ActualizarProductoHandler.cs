using Microsoft.Extensions.Logging;

namespace BackAlmancen.Application.Features.Productos;

public class ActualizarProductoHandler : IRequestHandler<ActualizarProductoCommand, Response<bool>>
{
    private readonly IRepository<Producto> _repository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ActualizarProductoHandler> _logger;
    public ActualizarProductoHandler(IRepository<Producto> repository, IMapper mapper, IWebHostEnvironment env, ILogger<ActualizarProductoHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _env = env;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(ActualizarProductoCommand request, CancellationToken cancellationToken)
    {
        var respuesta = new Response<bool>();

        var productoExistente = await _repository.GetByIdAsync(request.Id);
        if (productoExistente == null)
        {
            respuesta.Success = false;
            respuesta.MessageError = $"No se encontró el producto con ID {request.Id}.";
            return respuesta;
        }

        // 2. Verificar si viene una nueva imagen y es diferente a la que ya tenía registrada
        bool imagenCambio = !string.IsNullOrEmpty(request.ImageUrl)
                            && productoExistente.ImageUrl != request.ImageUrl;

        if (imagenCambio && !string.IsNullOrEmpty(productoExistente.ImageUrl))
        {
            var rutaRelativa = productoExistente.ImageUrl.TrimStart('/', '\\');
            var rutaWebRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var rutaArchivoAnterior = Path.Combine(rutaWebRoot, rutaRelativa);

            if (File.Exists(rutaArchivoAnterior))
            {
                try
                {
                    File.Delete(rutaArchivoAnterior);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"No se pudo borrar la imagen anterior: {ex.Message}");
                }
            }
        }

        var actualizar = _mapper.Map<ProductoDto, Producto>(request);
        var actualizado = await _repository.UpdateAsync(actualizar);

        if (!actualizado)
        {
            respuesta.Success = false;
            respuesta.Data = false;
            respuesta.MessageError = $"No se encontró el producto con ID {request.Id} para actualizar.";
            return respuesta;
        }

        respuesta.Data = true;
        return respuesta;
    }
}
