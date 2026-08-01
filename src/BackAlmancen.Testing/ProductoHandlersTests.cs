using AutoMapper;
using BackAlmancen.Application.Contracts;
using BackAlmancen.Application.Dtos;
using BackAlmancen.Application.Features.Productos;
using BackAlmancen.Application.Features.Productos.Commands;
using BackAlmancen.Application.Features.Productos.Queries;
using BackAlmancen.Domain.Models;
using Moq;

namespace BackAlmancen.Testing;

public class ProductoHandlersTests
{
    private readonly Mock<IRepository<Producto>> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    public ProductoHandlersTests()
    {
        _repositoryMock = new Mock<IRepository<Producto>>();
        _mapperMock = new Mock<IMapper>();
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
        var query = new RecuperaProductoPorIdQuery() { 
                                Id= 2
        };

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

        // Mock del Mapper: Convierte ProductoDto -> Producto
        _mapperMock.Setup(m => m.Map<ProductoDto, Producto>(It.IsAny<ProductoDto>()))
                   .Returns(productoEntidad);

        // Mock del Repository: Retorna la entidad agregada
        _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Producto>()))
                       .ReturnsAsync(productoEntidad);

        // Instanciar el Handler pasando AMBAS dependencias
        var handler = new CrearProductoHandler(_repositoryMock.Object, _mapperMock.Object);
        var command = new CrearProductoCommand { Producto = productoDto };

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.NotNull(resultado.Data);
        Assert.Equal("Hot Wheels", resultado.Data.Name);

        _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Producto>()), Times.Once);
        _mapperMock.Verify(m => m.Map<ProductoDto, Producto>(It.IsAny<ProductoDto>()), Times.Once);
    }

    [Fact]
    public async Task ActualizarProducto_DebeRetornarTrue_CuandoElProductoExiste()
    {
        // Arrange
        // 1. Objeto DTO que recibe el Command
        var productoDto = new ProductoDto
        {
            Name = "Barbie Divorciada",
            Price = 29.99m,
            Company = "Mattel"
        };

        // 2. Entidad resultante tras el mapeo
        var productoEntidad = new Producto
        {
            Id = 1,
            Name = "Barbie Divorciada",
            Price = 29.99m,
            Company = "Mattel"
        };

        // Mock del Repositorio: Actualización exitosa
        _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Producto>()))
                       .ReturnsAsync(true);

        // Mock del Mapper: Convierte ProductoDto -> Producto
        _mapperMock.Setup(m => m.Map<ProductoDto, Producto>(It.IsAny<ProductoDto>()))
                   .Returns(productoEntidad);

        var handler = new ActualizarProductoHandler(_repositoryMock.Object, _mapperMock.Object);

        var command = new ActualizarProductoCommand
        {
            Producto = productoDto
        };

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Data);
        _repositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Producto>()), Times.Once);
        _mapperMock.Verify(m => m.Map<ProductoDto, Producto>(It.IsAny<ProductoDto>()), Times.Once);
    }

    [Fact]
    public async Task EliminarProducto_DebeRetornarFalse_CuandoElProductoNoExiste()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.DeleteAsync(99))
                       .ReturnsAsync(false);

        var handler = new EliminarProductoHandler(_repositoryMock.Object);
        var command = new EliminarProductoQuery { Id = 99 };

        // Act
        var resultado = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(resultado);
        Assert.False(resultado.Data);
        Assert.False(resultado.Success);
        _repositoryMock.Verify(repo => repo.DeleteAsync(99), Times.Once);
    }
}
