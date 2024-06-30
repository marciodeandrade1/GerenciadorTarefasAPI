using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GerenciadorTarefasAPI.Controllers;
using GerenciadorTarefasAPI.DTOs;
using GerenciadorTarefasAPI.Services;

namespace GerenciadorTarefasAPI.Tests
{
    public class TarefaControllerTests
    {
        private readonly Mock<TarefaService> _tarefaServiceMock;
        private readonly Mock<ILogger<TarefaController>> _loggerMock;
        private readonly TarefaController _controller;

        public TarefaControllerTests()
        {
            _tarefaServiceMock = new Mock<TarefaService>(null, null);
            _loggerMock = new Mock<ILogger<TarefaController>>();
            _controller = new TarefaController(_tarefaServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetTarefasByProjetoId_ReturnsOkResult_WithListOfTasks()
        {
            // Arrange
            var tarefas = new List<TarefaDTO>
            {
                new TarefaDTO { Id = 1, Nome = "Tarefa 1", Descricao = "Descrição 1", ProjetoId = 1 },
                new TarefaDTO { Id = 2, Nome = "Tarefa 2", Descricao = "Descrição 2", ProjetoId = 1 }
            };

            _tarefaServiceMock.Setup(service => service.GetTarefasByProjetoIdAsync(1)).ReturnsAsync(tarefas);

            // Act
            var result = await _controller.GetTarefasByProjetoId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TarefaDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task CreateTarefa_ReturnsCreatedAtActionResult_WithTaskDTO()
        {
            // Arrange
            var tarefaDTO = new TarefaDTO { Id = 1, Nome = "Nova Tarefa", Descricao = "Nova Descrição", ProjetoId = 1 };

            // Act
            var result = await _controller.CreateTarefa(1, tarefaDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<TarefaDTO>(createdAtActionResult.Value);
            Assert.Equal(tarefaDTO.Nome, returnValue.Nome);
        }

        [Fact]
        public async Task UpdateTarefa_ReturnsNoContentResult_WhenTaskUpdated()
        {
            // Arrange
            var tarefaDTO = new TarefaDTO { Id = 1, Nome = "Tarefa Atualizada", Descricao = "Descrição Atualizada", ProjetoId = 1 };

            _tarefaServiceMock.Setup(service => service.UpdateTarefaAsync(1, tarefaDTO)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateTarefa(1, tarefaDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTarefa_ReturnsNoContentResult_WhenTaskRemoved()
        {
            // Arrange
            var tarefaId = 1;

            _tarefaServiceMock.Setup(service => service.DeleteTarefaAsync(tarefaId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTarefa(tarefaId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
