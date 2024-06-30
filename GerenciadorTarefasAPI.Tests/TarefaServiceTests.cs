using AutoMapper;
using GerenciadorTarefasAPI.DTOs;
using GerenciadorTarefasAPI.Models;
using GerenciadorTarefasAPI.Repositories;
using GerenciadorTarefasAPI.Services;
using Moq;

public class TarefaServiceTests
{
    private readonly Mock<ITarefaRepository> _mockTarefaRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly TarefaService _tarefaService;

    public TarefaServiceTests()
    {
        _mockTarefaRepository = new Mock<ITarefaRepository>();
        _mockMapper = new Mock<IMapper>();
        _tarefaService = new TarefaService(_mockTarefaRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetTarefasByProjetoIdAsync_ReturnsMappedTarefas()
    {
        // Arrange
        var projetoId = 1;
        var tarefas = new List<Tarefa> { new Tarefa { Id = 1, Descricao = "Teste descrição", ProjetoId = projetoId } };
        var tarefasDTO = new List<TarefaDTO> { new TarefaDTO { Id = 1, Nome = "Teste" } };

        _mockTarefaRepository.Setup(repo => repo.GetTarefasByProjetoIdAsync(projetoId))
            .ReturnsAsync(tarefas);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<TarefaDTO>>(tarefas))
            .Returns(tarefasDTO);

        // Act
        var result = await _tarefaService.GetTarefasByProjetoIdAsync(projetoId);

        // Assert
        Assert.Equal(tarefasDTO, result);
    }

    [Fact]
    public async Task CreateTarefaAsync_WhenTarefasCountExceedsLimit_ThrowsInvalidOperationException()
    {
        // Arrange
        var projetoId = 1;
        var tarefaDTO = new TarefaDTO { Nome = "Teste", Prioridade = "Alta" };

        _mockTarefaRepository.Setup(repo => repo.GetTarefasByProjetoIdAsync(projetoId))
            .ReturnsAsync(Enumerable.Range(1, 20).Select(i => new Tarefa { Id = i, ProjetoId = projetoId }));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _tarefaService.CreateTarefaAsync(projetoId, tarefaDTO));
    }

    [Fact]
    public async Task CreateTarefaAsync_AddsTarefaSuccessfully()
    {
        // Arrange
        var projetoId = 1;
        var tarefaDTO = new TarefaDTO { Nome = "Teste", Prioridade = "Alta" };
        var tarefa = new Tarefa { Id = 1, Descricao = "Teste", Prioridade = "Alta", ProjetoId = projetoId };

        _mockTarefaRepository.Setup(repo => repo.GetTarefasByProjetoIdAsync(projetoId))
            .ReturnsAsync(new List<Tarefa>());
        _mockMapper.Setup(mapper => mapper.Map<Tarefa>(tarefaDTO))
            .Returns(tarefa);

        // Act
        await _tarefaService.CreateTarefaAsync(projetoId, tarefaDTO);

        // Assert
        _mockTarefaRepository.Verify(repo => repo.AddTarefaAsync(It.IsAny<Tarefa>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTarefaAsync_WhenTarefaNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var tarefaId = 1;
        var tarefaDTO = new TarefaDTO { Id = tarefaId, Nome = "Teste Atualizado", Prioridade = "Alta" };

        _mockTarefaRepository.Setup(repo => repo.GetTarefaByIdAsync(tarefaId))
            .ReturnsAsync((Tarefa)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _tarefaService.UpdateTarefaAsync(tarefaId, tarefaDTO));
    }

    [Fact]
    public async Task UpdateTarefaAsync_WhenPriorityChanged_ThrowsInvalidOperationException()
    {
        // Arrange
        var tarefaId = 1;
        var tarefaExistente = new Tarefa { Id = tarefaId, Descricao = "Teste", Prioridade = "Alta" };
        var tarefaDTO = new TarefaDTO { Id = tarefaId, Nome = "Teste Atualizado", Prioridade = "Baixa" };

        _mockTarefaRepository.Setup(repo => repo.GetTarefaByIdAsync(tarefaId))
            .ReturnsAsync(tarefaExistente);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _tarefaService.UpdateTarefaAsync(tarefaId, tarefaDTO));
    }

    [Fact]
    public async Task UpdateTarefaAsync_UpdatesTarefaSuccessfully()
    {
        // Arrange
        var tarefaId = 1;
        var tarefaExistente = new Tarefa { Id = tarefaId, Descricao = "Teste", Prioridade = "Alta" };
        var tarefaDTO = new TarefaDTO { Id = tarefaId, Nome = "Teste Atualizado", Prioridade = "Alta", Status = "Em Progresso" };

        _mockTarefaRepository.Setup(repo => repo.GetTarefaByIdAsync(tarefaId))
            .ReturnsAsync(tarefaExistente);

        // Act
        await _tarefaService.UpdateTarefaAsync(tarefaId, tarefaDTO);

        // Assert
        _mockTarefaRepository.Verify(repo => repo.UpdateTarefaAsync(It.IsAny<Tarefa>()), Times.Once);
        _mockTarefaRepository.Verify(repo => repo.AddTaskHistoryAsync(It.IsAny<TarefaHistorico>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTarefaAsync_DeletesTarefaSuccessfully()
    {
        // Arrange
        var tarefaId = 1;

        // Act
        await _tarefaService.DeleteTarefaAsync(tarefaId);

        // Assert
        _mockTarefaRepository.Verify(repo => repo.RemoveTarefaAsync(tarefaId), Times.Once);
    }
}
