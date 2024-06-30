using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GerenciadorTarefasAPI.Controllers;
using GerenciadorTarefasAPI.DTOs;
using GerenciadorTarefasAPI.Models;
using GerenciadorTarefasAPI.Repositories;
using GerenciadorTarefasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class TarefaControllerTests
{
    private readonly Mock<ITarefaRepository> _mockTarefaRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<TarefaController>> _mockLogger;
    private readonly TarefaService _tarefaService;
    private readonly TarefaController _tarefaController;

    public TarefaControllerTests()
    {
        _mockTarefaRepository = new Mock<ITarefaRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<TarefaController>>();
        _tarefaService = new TarefaService(_mockTarefaRepository.Object, _mockMapper.Object);
        _tarefaController = new TarefaController(_tarefaService, _mockLogger.Object);
    }

    [Fact]
    public async Task GetTarefasByProjetoId_ReturnsOkResult_WithListOfTarefas()
    {
        // Arrange
        var projetoId = 1;
        var tarefaDTOs = new List<TarefaDTO> { new TarefaDTO { Id = 1, Nome = "Teste" } };
        _mockTarefaRepository.Setup(repo => repo.GetTarefasByProjetoIdAsync(projetoId)).ReturnsAsync(new List<Tarefa> { new Tarefa { Id = 1, Descricao = "Teste" } });
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<TarefaDTO>>(It.IsAny<IEnumerable<Tarefa>>())).Returns(tarefaDTOs);

        // Act
        var result = await _tarefaController.GetTarefasByProjetoId(projetoId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<TarefaDTO>>(okResult.Value);
        Assert.Equal(tarefaDTOs, returnValue);
    }

    [Fact]
    public async Task CreateTarefa_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var projetoId = 1;
        var tarefaDTO = new TarefaDTO { Id = 1, Nome = "Teste" };
        _mockTarefaRepository.Setup(repo => repo.GetTarefasByProjetoIdAsync(projetoId)).ReturnsAsync(new List<Tarefa>());
        _mockMapper.Setup(mapper => mapper.Map<Tarefa>(tarefaDTO)).Returns(new Tarefa { Id = 1, Descricao = "Teste" });

        // Act
        var result = await _tarefaController.CreateTarefa(projetoId, tarefaDTO);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_tarefaController.GetTarefasByProjetoId), createdAtActionResult.ActionName);
        Assert.Equal(projetoId, createdAtActionResult.RouteValues["projetoId"]);
        Assert.Equal(tarefaDTO, createdAtActionResult.Value);
    }

    [Fact]
    public async Task UpdateTarefa_ReturnsNoContentResult()
    {
        // Arrange
        var tarefaId = 1;
        var tarefaDTO = new TarefaDTO { Id = tarefaId, Nome = "Teste Atualizado" };
        _mockTarefaRepository.Setup(repo => repo.GetTarefaByIdAsync(tarefaId)).ReturnsAsync(new Tarefa { Id = tarefaId, Descricao = "Teste" });

        // Act
        var result = await _tarefaController.UpdateTarefa(tarefaId, tarefaDTO);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteTarefa_ReturnsNoContentResult()
    {
        // Arrange
        var tarefaId = 1;

        // Act
        var result = await _tarefaController.DeleteTarefa(tarefaId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
