namespace BackAlmancen.Application.Features.Productos;

public class EliminarProductoHandler : IRequestHandler<EliminarProductoQuery, Response<bool>>
{
    private readonly IRepository<Producto> _repository;

    public EliminarProductoHandler(IRepository<Producto> repository)
    {
        _repository = repository;
    }

    public async Task<Response<bool>> Handle(EliminarProductoQuery request, CancellationToken cancellationToken)
    {
        var respuesta = new Response<bool>();
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
