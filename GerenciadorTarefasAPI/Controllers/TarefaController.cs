using System.Collections.Generic;
using System.Threading.Tasks;
using GerenciadorTarefasAPI.Models;
using GerenciadorTarefasAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorTarefasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaService _tarefaService;

        public TarefaController(TarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpGet("projeto/{projetoId}")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefasByProjetoId(int projetoId)
        {
            return Ok(await _tarefaService.GetTarefasByProjetoIdAsync(projetoId));
        }

        [HttpPost("projeto/{projetoId}")]
        public async Task<ActionResult> CreateTarefa(int projetoId, Tarefa tarefa)
        {
            await _tarefaService.CreateTarefaAsync(projetoId, tarefa);
            return CreatedAtAction(nameof(GetTarefasByProjetoId), new { projetoId = projetoId }, tarefa);
        }

        [HttpPut("{tarefaId}")]
        public async Task<ActionResult> UpdateTarefa(int tarefaId, Tarefa tarefa)
        {
            try
            {
                await _tarefaService.UpdateTarefaAsync(tarefaId, tarefa);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{tarefaId}")]
        public async Task<ActionResult> DeleteTarefa(int tarefaId)
        {
            await _tarefaService.DeleteTarefaAsync(tarefaId);
            return NoContent();
        }
    }
}
