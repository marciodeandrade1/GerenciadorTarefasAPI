using GerenciadorTarefasAPI.DTOs;

namespace GerenciadorTarefasAPI.Services
{
    public interface IProjetoService
    {
        Task<IEnumerable<ProjetoDTO>> GetProjetosAsync();
        Task CreateProjetoAsync(ProjetoDTO projetoDTO);
        Task RemoveProjetoAsync(int id);
    }
}
