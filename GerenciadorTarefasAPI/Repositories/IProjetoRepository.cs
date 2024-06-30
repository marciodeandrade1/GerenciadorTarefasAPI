using GerenciadorTarefasAPI.Models;

namespace GerenciadorTarefasAPI.Repositories
{
    public interface IProjetoRepository
    {
        Task<IEnumerable<Projeto>> GetProjetosAsync();
        Task<Projeto> GetProjetoByIdAsync(int id);
        Task AddProjetoAsync(Projeto projeto);
        Task UpdateProjetoAsync(Projeto projeto);
        Task RemoveProjetoAsync(int id);
    }
}
