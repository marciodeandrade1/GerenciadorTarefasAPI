using AutoMapper;
using GerenciadorTarefasAPI.DTOs;
using GerenciadorTarefasAPI.Models;
using GerenciadorTarefasAPI.Repositories;
using GerenciadorTarefasAPI.Services;
using Moq;

namespace GerenciadorTarefasAPI.Tests
{
    public class ProjetoServiceTests
    {
        private readonly Mock<IProjetoRepository> _projetoRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ProjetoService _projetoService;

        public ProjetoServiceTests()
        {
            _projetoRepositoryMock = new Mock<IProjetoRepository>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<GerenciadorTarefasAPI.Mappings.MappingProfile>());
            _mapper = configuration.CreateMapper();

            _projetoService = new ProjetoService(_projetoRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetProjetosAsync_ShouldReturnAllProjetos()
        {
            // Arrange
            var projetos = new List<Projeto>
            {
                new() { Id = 1, Nome = "Projeto 1"  },
                new Projeto { Id = 2, Nome = "Projeto 2"  }
            };

            _projetoRepositoryMock.Setup(repo => repo.GetProjetosAsync()).ReturnsAsync(projetos);

            // Act
            var result = await _projetoService.GetProjetosAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Projeto 1", result.First().Nome);
        }

        [Fact]
        public async Task CreateProjetoAsync_ShouldAddProjeto()
        {
            // Arrange
            var projetoDTO = new ProjetoDTO { Nome = "Projeto Teste" };

            // Act
            await _projetoService.CreateProjetoAsync(projetoDTO);

            // Assert
            _projetoRepositoryMock.Verify(repo => repo.AddProjetoAsync(It.IsAny<Projeto>()), Times.Once);
        }
    }
}
