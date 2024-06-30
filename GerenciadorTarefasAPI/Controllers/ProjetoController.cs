using GerenciadorTarefasAPI.DTOs;
using GerenciadorTarefasAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorTarefasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        private readonly ProjetoService _projetoService;
        private readonly ILogger<ProjetoController> _logger;

        public ProjetoController(ProjetoService projetoService, ILogger<ProjetoController> logger)
        {
            _projetoService = projetoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjetoDTO>>> GetProjetos()
        {
            _logger.LogInformation("Obtendo todos os projetos");
            var projetos = await _projetoService.GetProjetosAsync();
            return Ok(projetos);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProjeto(ProjetoDTO projetoDTO)
        {
            _logger.LogInformation("Criando um novo projeto");
            await _projetoService.CreateProjetoAsync(projetoDTO);
            return CreatedAtAction(nameof(GetProjetos), new { id = projetoDTO.Id }, projetoDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveProjeto(int id)
        {
            try
            {
                _logger.LogInformation($"Removendo projeto com id {id}");
                await _projetoService.RemoveProjetoAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Tentativa de remover projeto com tarefas pendentes");
                return BadRequest(ex.Message);
            }
        }
    }
}
