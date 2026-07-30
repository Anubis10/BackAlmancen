namespace BackAlmancen.Application.Features.Productos.Commands;

public class ActualizarProductoCommand : IRequest<Response<bool>>
{
    public ProductoDto Producto { get; set; } = null!;
}
