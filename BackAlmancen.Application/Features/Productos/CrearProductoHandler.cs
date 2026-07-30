namespace BackAlmancen.Application.Features.Productos;

public class CrearProductoHandler : IRequestHandler<CrearProductoCommand, Response<Producto>>
{
    private readonly IRepository<Producto> _repository;
    private readonly IMapper _mapper;
    public CrearProductoHandler(IRepository<Producto> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Response<Producto>> Handle(CrearProductoCommand request, CancellationToken cancellationToken)
    {
        var respuesta = new Response<Producto>();
        var nuevo = _mapper.Map<ProductoDto, Producto>(request.Producto);
        respuesta.Data = await _repository.AddAsync(nuevo);
        return respuesta;
    }
}
