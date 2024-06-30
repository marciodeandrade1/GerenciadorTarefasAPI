using System.Collections.Generic;
using System.Threading.Tasks;
using GerenciadorTarefasAPI.Models;
using GerenciadorTarefasAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorTarefasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetoController : ControllerBase
    {
        private readonly ProjetoService _projetoService;

        public ProjetoController(ProjetoService projetoService)
        {
            _projetoService = projetoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Projeto>>> GetProjetos()
        {
            return Ok(await _projetoService.GetProjetosAsync());
        }

        [HttpPost]
        public async Task<ActionResult> CreateProjeto(Projeto projeto)
        {
            await _projetoService.CreateProjetoAsync(projeto);
            return CreatedAtAction(nameof(GetProjetos), new { id = projeto.Id }, projeto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveProjeto(int id)
        {
            try
            {
                await _projetoService.RemoveProjetoAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
