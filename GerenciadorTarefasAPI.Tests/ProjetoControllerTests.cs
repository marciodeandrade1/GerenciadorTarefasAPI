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
    public class ProjetoControllerTests
    {
        private readonly Mock<ProjetoService> _projetoServiceMock;
        private readonly Mock<ILogger<ProjetoController>> _loggerMock;
        private readonly ProjetoController _controller;

        public ProjetoControllerTests()
        {
            _projetoServiceMock = new Mock<ProjetoService>(null, null);
            _loggerMock = new Mock<ILogger<ProjetoController>>();
            _controller = new ProjetoController(_projetoServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetProjetos_ReturnsOkResult_WithListOfProjects()
        {
            // Arrange
            var projetos = new List<ProjetoDTO>
            {
                new ProjetoDTO { Id = 1, Nome = "Projeto 1", Descricao = "Descrição 1" },
                new ProjetoDTO { Id = 2, Nome = "Projeto 2", Descricao = "Descrição 2" }
            };

            _projetoServiceMock.Setup(service => service.GetProjetosAsync()).ReturnsAsync(projetos);

            // Act
            var result = await _controller.GetProjetos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ProjetoDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task CreateProjeto_ReturnsCreatedAtActionResult_WithProjectDTO()
        {
            // Arrange
            var projetoDTO = new ProjetoDTO { Id = 1, Nome = "Novo Projeto", Descricao = "Nova Descrição" };

            // Act
            var result = await _controller.CreateProjeto(projetoDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ProjetoDTO>(createdAtActionResult.Value);
            Assert.Equal(projetoDTO.Nome, returnValue.Nome);
        }

        [Fact]
        public async Task RemoveProjeto_ReturnsNoContentResult_WhenProjectRemoved()
        {
            // Arrange
            var projetoId = 1;

            _projetoServiceMock.Setup(service => service.RemoveProjetoAsync(projetoId)).Returns(Task.CompletedTask);

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
            var errorMessage = "Attempted to remove project with pending tasks";

            _projetoServiceMock.Setup(service => service.RemoveProjetoAsync(projetoId)).Throws(new InvalidOperationException(errorMessage));

            // Act
            var result = await _controller.RemoveProjeto(projetoId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }
    }
}
