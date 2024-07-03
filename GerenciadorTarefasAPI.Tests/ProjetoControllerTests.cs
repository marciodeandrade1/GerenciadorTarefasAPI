using GerenciadorTarefasAPI.DTOs;
using GerenciadorTarefasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Text;

public class ProjetoControllerTests
{
    private readonly Mock<IProjetoService> _mockProjetoService;
    private readonly Mock<ILogger<ProjetoController>> _mockLogger;
    private readonly Mock<IDistributedCache> _mockDistributedCache;
    private readonly ProjetoController _controller;

    public ProjetoControllerTests()
    {
        _mockProjetoService = new Mock<IProjetoService>();
        _mockLogger = new Mock<ILogger<ProjetoController>>();
        _mockDistributedCache = new Mock<IDistributedCache>();
        _controller = new ProjetoController(_mockProjetoService.Object, _mockLogger.Object, _mockDistributedCache.Object);
    }

    [Fact]
    public async Task GetProjetos_ReturnsOkResult_WithListOfProjetos()
    {
        // Arrange
        var projetos = new List<ProjetoDTO>
        {
            new ProjetoDTO { Id = 1, Nome = "Projeto 1", Descricao = "Descricao 1" },
            new ProjetoDTO { Id = 2, Nome = "Projeto 2", Descricao = "Descricao 2" }
        };

        _mockProjetoService.Setup(service => service.GetProjetosAsync())
                           .ReturnsAsync(projetos);

        var cachedData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(projetos));

        // Ensure cache returns null initially
        _mockDistributedCache.Setup(cache => cache.GetAsync("projetoList", default))
                             .ReturnsAsync((byte[])null);

        // Ensure cache is set with the correct data
        _mockDistributedCache.Setup(cache => cache.SetAsync("projetoList", It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), default))
                             .Verifiable();

        // Act
        var result = await _controller.GetProjetos();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<ProjetoDTO>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
        Assert.Equal("Projeto 1", returnValue[0].Nome);
        Assert.Equal("Projeto 2", returnValue[1].Nome);

        _mockDistributedCache.Verify();
    }

    [Fact]
    public async Task CreateProjeto_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var projetoDTO = new ProjetoDTO { Id = 3, Nome = "Novo Projeto", Descricao = "Nova Descricao" };

        // Act
        var result = await _controller.CreateProjeto(projetoDTO);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetProjetos), createdAtActionResult.ActionName);
        Assert.Equal(projetoDTO, createdAtActionResult.Value);
    }

    [Fact]
    public async Task RemoveProjeto_ReturnsNoContent_WhenProjectRemoved()
    {
        // Arrange
        var projetoId = 1;

        _mockProjetoService.Setup(service => service.RemoveProjetoAsync(projetoId))
                           .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RemoveProjeto(projetoId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task RemoveProjeto_ReturnsBadRequest_WhenProjectHasPendingTasks()
    {
        // Arrange
        var projetoId = 1;
        var errorMessage = "Tentativa de remover projeto com tarefas pendentes";

        _mockProjetoService.Setup(service => service.RemoveProjetoAsync(projetoId))
                           .ThrowsAsync(new InvalidOperationException(errorMessage));

        // Act
        var result = await _controller.RemoveProjeto(projetoId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(errorMessage, badRequestResult.Value);
    }
}
