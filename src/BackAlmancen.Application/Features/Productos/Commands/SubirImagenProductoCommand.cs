

namespace BackAlmancen.Application.Features.Productos.Commands;

public class SubirImagenProductoCommand : IRequest<Response<string>>
{
    public IFormFile Archivo { get; set; } = null!;
}
