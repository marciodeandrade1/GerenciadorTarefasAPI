using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GerenciadorTarefasAPI.Services;
using GerenciadorTarefasAPI.Repositories;
using GerenciadorTarefasAPI.Models;
using AutoMapper;
using GerenciadorTarefasAPI.DTOs;

namespace GerenciadorTarefasAPI.Tests
{
    public class TarefaServiceTests
    {
        private readonly Mock<ITarefaRepository> _tarefaRepositoryMock;
        private readonly IMapper _mapper;
        private readonly TarefaService _tarefaService;

        public TarefaServiceTests()
        {
            _tarefaRepositoryMock = new Mock<ITarefaRepository>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<GerenciadorTarefasAPI.Mappings.MappingProfile>());
            _mapper = configuration.CreateMapper();

            _tarefaService = new TarefaService(_tarefaRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetTarefasAsync_ShouldReturnAllTarefas()
        {
            // Arrange
            var tarefas = new List<Tarefa>
            {
                //new() { Id = 1, Status = "1"  },
                new Tarefa { Id = 2, Descricao = "Projeto 2"  }
            };

            _tarefaRepositoryMock.Setup(repo => repo.GetTarefasByProjetoIdAsync(2)).ReturnsAsync(tarefas);

            // Act
            var result = await _tarefaService.GetTarefasByProjetoIdAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Projeto 1", result.First().Nome);
        }

        //[Fact]
        //public async Task CreateTarefaAsync_ShouldAddTarefa(TarefaDTO)
        //{
        //    // Arrange
        //    var tarefaDTO = new TarefaDTO { Nome = "Tarefa Teste",  };

        //    // Act
        //    await _tarefaService.CreateTarefaAsync(tarefaDTO);

        //    // Assert
        //    _tarefaRepositoryMock.Verify(repo => repo.AddTarefaAsync(It.IsAny<Tarefa>()), Times.Once);
        //}
    }
}
