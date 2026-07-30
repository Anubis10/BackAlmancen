namespace BackAlmancen.Application.Features.Productos;

public class SubirImagenProductoHandler : IRequestHandler<SubirImagenProductoCommand, Response<string>>
{
    private readonly IWebHostEnvironment _env;

    public SubirImagenProductoHandler(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<Response<string>> Handle(SubirImagenProductoCommand request, CancellationToken cancellationToken)
    {
        var respuesta = new Response<string>();

        if (request.Archivo == null || request.Archivo.Length == 0)
        {
            respuesta.Success = false;
            respuesta.MessageError = "No se ha seleccionado ningún archivo.";
            return respuesta;
        }

        var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(request.Archivo.FileName).ToLowerInvariant();

        if (!extensionesPermitidas.Contains(extension))
        {
            respuesta.Success = false;
            respuesta.MessageError = "Formato de imagen no permitido. Utiliza JPG, PNG o WEBP.";
            return respuesta;
        }

        var directorioUploads = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
        if (!Directory.Exists(directorioUploads)) Directory.CreateDirectory(directorioUploads);

        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaCompleta = Path.Combine(directorioUploads, nombreArchivo);

        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            await request.Archivo.CopyToAsync(stream, cancellationToken);
        }

        respuesta.Data = $"/uploads/{nombreArchivo}";
        return respuesta;
    }
}
