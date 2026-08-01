using AutoMapper;
using BackAlmancen.Application.Contracts;
using BackAlmancen.Application.Dtos;
using BackAlmancen.Application.Features.Productos;
using BackAlmancen.Application.Features.Productos.Commands;
using BackAlmancen.Application.Features.Productos.Queries;
using BackAlmancen.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BackAlmancen.Testing;

public class ProductoHandlersTests
{
    private readonly Mock<IRepository<Producto>> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IWebHostEnvironment> _envMock;

    public ProductoHandlersTests()
    {
        _repositoryMock = new Mock<IRepository<Producto>>();
        _mapperMock = new Mock<IMapper>();
        _envMock = new Mock<IWebHostEnvironment>();

               _envMock.Setup(e => e.WebRootPath).Returns("C:\\fake_wwwroot");
    }

    [Fact]
    public async Task RecuperarTodos_DebeRetornarListaDeProductos()
    {
        // Arrange
        var productosDummy = new List<Producto>
        {
            new Producto { Id = 1, Name = "Barbie Developer", Price = 25.99m, Company = "Mattel" },
            new Producto { Id = 2, Name = "xyc", Price = 75.50m, Company = "Marvel" }
        };

        _repositoryMock.Setup(repo => repo.GetAllAsync())
                       .ReturnsAsync(productosDummy);

        var handler = new RecuperarProductosHandler(_repositoryMock.Object);
        var query = new RecuperaProductosQuery();

        // Act
        var resultado = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.NotNull(resultado.Data);
        Assert.Equal(2, resultado.Data.Count());
        _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task RecuperarPorId_DebeRetornarProducto_CuandoExiste()
    {
        // Arrange
        var productoExistente = new Producto { Id = 2, Name = "Spider-man", Price = 75.50m, Company = "Marvel" };

        _repositoryMock.Setup(repo => repo.GetByIdAsync(2))
                       .ReturnsAsync(productoExistente);

        var handler = new RecuperarProductoPorIdHandler(_repositoryMock.Object);
        var query = new RecuperaProductoPorIdQuery { Id = 2 };

        // Act
        var resultado = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.NotNull(resultado.Data);
        Assert.Equal(2, resultado.Data.Id);
        Assert.Equal("Spider-man", resultado.Data.Name);
        Assert.True(resultado.Success);
    }

    [Fact]
    public async Task CrearProducto_DebeAgregarYRetornarProducto()
    {
        // Arrange
        var productoDto = new ProductoDto
        {
            Name = "Hot Wheels",
            Price = 15.00m,
            Company = "Mattel"
        };

        var productoEntidad = new Producto
        {
            Id = 4,
            Name = "Hot Wheels",
            Price = 15.00m,
            Company = "Mattel"
        };

        _mapperMock.Setup(m => m.Map<ProductoDto, Producto>(It.IsAny<ProductoDto>()))
                   .Returns(productoEntidad);

        _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Producto>()))
                       .ReturnsAsync(productoEntidad);

        var loggerMock = new Mock<ILogger<CrearProductoHandler>>();

        // Handler con las dependencias actualizadas
        var handler = new CrearProductoHandler(_repositoryMock.Object, _mapperMock.Object);

        // Mapeo directo del DTO o propiedades según tu definición de Command
        var command = new CrearProductoCommand
        {
            Name = productoDto.Name,
            Price = productoDto.Price,
            Company = productoDto.Company
        };

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.NotNull(resultado.Data);
        Assert.Equal("Hot Wheels", resultado.Data.Name);

        _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Producto>()), Times.Once);
    }

    [Fact]
    public async Task ActualizarProducto_DebeRetornarTrue_CuandoElProductoExiste()
    {
        // Arrange
        var productoExistente = new Producto
        {
            Id = 1,
            Name = "Barbie Antigua",
            Price = 25.00m,
            Company = "Mattel",
            ImageUrl = "/uploads/anterior.jpg"
        };

        var productoMapeado = new Producto
        {
            Id = 1,
            Name = "Barbie Divorciada",
            Price = 29.99m,
            Company = "Mattel",
            ImageUrl = "/uploads/nueva.jpg"
        };

        var command = new ActualizarProductoCommand
        {
            Id = 1,
            Name = "Barbie Divorciada",
            Price = 29.99m,
            Company = "Mattel",
            ImageUrl = "/uploads/nueva.jpg"
        };

        // 1. Configurar GetByIdAsync para simular que el producto sí existe en BD
        _repositoryMock.Setup(repo => repo.GetByIdAsync(command.Id))
                       .ReturnsAsync(productoExistente);

        // 2. Configurar el Mapper para recibir el ActualizarProductoCommand
        _mapperMock.Setup(m => m.Map<ProductoDto, Producto>(It.IsAny<ActualizarProductoCommand>()))
                   .Returns(productoMapeado);

        // 3. Configurar UpdateAsync
        _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Producto>()))
                       .ReturnsAsync(true);

        var loggerMock = new Mock<ILogger<ActualizarProductoHandler>>();
        var handler = new ActualizarProductoHandler(_repositoryMock.Object, _mapperMock.Object, _envMock.Object, loggerMock.Object);

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Data);
        _repositoryMock.Verify(repo => repo.GetByIdAsync(command.Id), Times.Once);
        _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Producto>()), Times.Once);
    }

    [Fact]
    public async Task EliminarProducto_DebeRetornarFalse_CuandoElProductoNoExiste()
    {
        // Arrange
        // Como el producto no existe, GetByIdAsync debe retornar null
        _repositoryMock.Setup(repo => repo.GetByIdAsync(99))
                       .ReturnsAsync((Producto?)null);

        var loggerMock = new Mock<ILogger<EliminarProductoHandler>>();
        var handler = new EliminarProductoHandler(_repositoryMock.Object, _envMock.Object, loggerMock.Object);
        var query = new EliminarProductoQuery { Id = 99 };

        // Act
        var resultado = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.False(resultado.Success); // El Handler retorna Success = false cuando no lo encuentra
        _repositoryMock.Verify(repo => repo.GetByIdAsync(99), Times.Once);

        // Verificamos que NUNCA intentó borrar en BD porque ya sabía que no existía
        _repositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
        
}