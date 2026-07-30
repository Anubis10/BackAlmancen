namespace BackAlmancen.Application.Features.Productos.Queries;

public class RecuperaProductoPorIdQuery : IRequest<Response<Producto>>
{
    public int Id { get; set; }
    
}
