namespace BackAlmancen.Application.Features.Productos
{
    public class RecuperarProductosHandler : IRequestHandler<RecuperaProductosQuery, Response<IEnumerable<Producto>>>
    {
        private readonly IRepository<Producto> _repository;

        public RecuperarProductosHandler(IRepository<Producto> repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<Producto>>> Handle(RecuperaProductosQuery request, CancellationToken cancellationToken)
        {
            var respuesta = new Response<IEnumerable<Producto>>();

            respuesta.Data = await _repository.GetAllAsync();

            return respuesta;
        }
    }
}
