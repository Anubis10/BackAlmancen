namespace BackAlmancen.Application.Features.Productos.Queries;

public class EliminarProductoQuery : IRequest<Response<bool>>
{
    public int Id { get; set; }
}
