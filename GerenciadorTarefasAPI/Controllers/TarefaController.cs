using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using GerenciadorTarefasAPI.DTOs;
using GerenciadorTarefasAPI.Services;

namespace GerenciadorTarefasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaService _tarefaService;
        private readonly ILogger<TarefaController> _logger;

        public TarefaController(TarefaService tarefaService, ILogger<TarefaController> logger)
        {
            _tarefaService = tarefaService;
            _logger = logger;
        }

        [HttpGet("{projetoId}")]
        public async Task<ActionResult<IEnumerable<TarefaDTO>>> GetTarefasByProjetoId(int projetoId)
        {
            _logger.LogInformation($"Obtendo tarefas para o projeto id {projetoId}");
            var tarefas = await _tarefaService.GetTarefasByProjetoIdAsync(projetoId);
            return Ok(tarefas);
        }

        [HttpPost("{projetoId}")]
        public async Task<ActionResult> CreateTarefa(int projetoId, TarefaDTO tarefaDTO)
        {
            _logger.LogInformation($"Criando uma nova tarefa para o projeto id {projetoId}");
            await _tarefaService.CreateTarefaAsync(projetoId, tarefaDTO);
            return CreatedAtAction(nameof(GetTarefasByProjetoId), new { projetoId = projetoId }, tarefaDTO);
        }

        [HttpPut("{tarefaId}")]
        public async Task<ActionResult> UpdateTarefa(int tarefaId, TarefaDTO tarefaDTO)
        {
            _logger.LogInformation($"Atualizando tarefa id {tarefaId}");
            await _tarefaService.UpdateTarefaAsync(tarefaId, tarefaDTO);
            return NoContent();
        }

        [HttpDelete("{tarefaId}")]
        public async Task<ActionResult> DeleteTarefa(int tarefaId)
        {
            _logger.LogInformation($"Excluindo tarefa id {tarefaId}");
            await _tarefaService.DeleteTarefaAsync(tarefaId);
            return NoContent();
        }
    }
}
