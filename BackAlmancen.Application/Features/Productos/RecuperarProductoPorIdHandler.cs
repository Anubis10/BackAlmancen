namespace BackAlmancen.Application.Features.Productos;

public class RecuperarProductoPorIdHandler : IRequestHandler<RecuperaProductoPorIdQuery, Response<Producto>>
{
    private readonly IRepository<Producto> _repository;

    public RecuperarProductoPorIdHandler(IRepository<Producto> repository)
    {
        _repository = repository;
    }

    public async Task<Response<Producto>> Handle(RecuperaProductoPorIdQuery request, CancellationToken cancellationToken)
    {
        var respuesta = new Response<Producto>();
        var producto = await _repository.GetByIdAsync(request.Id);

        if (producto == null)
        {
            respuesta.Success = false;
            respuesta.MessageError= $"Producto con ID {request.Id} no encontrado.";
            return respuesta;
        }

        respuesta.Data = producto;
        return respuesta;
    }
}
