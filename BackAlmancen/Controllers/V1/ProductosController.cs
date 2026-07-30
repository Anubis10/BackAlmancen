using Asp.Versioning;
using BackAlmancen.Application.Features.Productos.Commands;
using BackAlmancen.Application.Features.Productos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackAlmancen.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductosController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RecuperarTodos([FromQuery] RecuperaProductosQuery query)
        {
            var productos = await _mediator.Send(query);

            return Ok(productos);
        }

        [HttpGet("porid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RecuperarPorID([FromQuery] RecuperaProductoPorIdQuery query)
        {
            var respuesta = await _mediator.Send(query);

            if (respuesta.StatusCode == 500)
            {
                return BadRequest(respuesta);
            }

            return Ok(respuesta);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearProducto([FromBody] CrearProductoCommand command)
        {
            var respuesta = await _mediator.Send(command);
            if (respuesta.StatusCode == 500)
            {
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActualizarProducto( [FromBody] ActualizarProductoCommand command)
        {
            var respuesta = await _mediator.Send(command);
            if (respuesta.StatusCode == 500)
            {
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }

        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromQuery] EliminarProductoQuery command)
        {
            var respuesta = await _mediator.Send(command);
            if (respuesta.StatusCode == 500)
            {
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }

        [HttpPost("cargar-imagen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage([FromForm] SubirImagenProductoCommand command)
        {
            var respuesta = await _mediator.Send(command);
            if (respuesta.StatusCode == 500)
            {
                return BadRequest(respuesta);
            }
            return Ok(respuesta);
        }
    }
}
