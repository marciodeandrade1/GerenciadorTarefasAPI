using GerenciadorTarefasAPI.DTOs;
using GerenciadorTarefasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class ProjetoController : ControllerBase
{
    private readonly IProjetoService _projetoService;
    private readonly ILogger<ProjetoController> _logger;
    private readonly IDistributedCache _distributedCache;

    public ProjetoController(IProjetoService projetoService, ILogger<ProjetoController> logger, IDistributedCache distributedCache)
    {
        _projetoService = projetoService;
        _logger = logger;
        _distributedCache = distributedCache;
    }

   [HttpGet]
public async Task<ActionResult<IEnumerable<ProjetoDTO>>> GetProjetos()
{
    var cacheKey = "projetoList";
    string serializedProjetoList;
    List<ProjetoDTO> projetoList;
    var redisProjetoList = await _distributedCache.GetAsync(cacheKey);

    if (redisProjetoList != null)
    {
        serializedProjetoList = Encoding.UTF8.GetString(redisProjetoList);
        projetoList = JsonConvert.DeserializeObject<List<ProjetoDTO>>(serializedProjetoList);
    }
    else
    {
            projetoList = (await _projetoService.GetProjetosAsync()).ToList();
            serializedProjetoList = JsonConvert.SerializeObject(projetoList);
        var redisOptions = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(60)); // Ajuste conforme necessário
        await _distributedCache.SetStringAsync(cacheKey, serializedProjetoList, redisOptions);
    }

    _logger.LogInformation("Obtendo todos os projetos");
    return Ok(projetoList);
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
