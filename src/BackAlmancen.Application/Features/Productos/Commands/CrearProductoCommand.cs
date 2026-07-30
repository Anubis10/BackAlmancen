namespace BackAlmancen.Application.Features.Productos.Commands
{
    public class CrearProductoCommand : IRequest<Response<Producto>>
    {
        public ProductoDto Producto { get; set; } = null!;
    }
}
