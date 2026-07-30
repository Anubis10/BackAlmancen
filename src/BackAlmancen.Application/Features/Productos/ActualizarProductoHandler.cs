namespace BackAlmancen.Application.Features.Productos;

public class ActualizarProductoHandler : IRequestHandler<ActualizarProductoCommand, Response<bool>>
{
    private readonly IRepository<Producto> _repository;
    private readonly IMapper _mapper;
    public ActualizarProductoHandler(IRepository<Producto> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Response<bool>> Handle(ActualizarProductoCommand request, CancellationToken cancellationToken)
    {
        var respuesta = new Response<bool>();
        var actualizar = _mapper.Map<ProductoDto, Producto>(request.Producto);
        var actualizado = await _repository.UpdateAsync(actualizar);

        if (!actualizado)
        {
            respuesta.Success = false;
            respuesta.Data = false;
            respuesta.MessageError = $"No se encontró el producto con ID {request.Producto.Id} para actualizar.";
            return respuesta;
        }

        respuesta.Data = true;
        return respuesta;
    }
}
