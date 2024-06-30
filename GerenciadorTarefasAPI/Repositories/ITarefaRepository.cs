using GerenciadorTarefasAPI.Models;

namespace GerenciadorTarefasAPI.Repositories
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> GetTarefasByProjetoIdAsync(int projetoId);
        Task<Tarefa> GetTarefaByIdAsync(int id);
        Task AddTarefaAsync(Tarefa tarefa);
        Task UpdateTarefaAsync(Tarefa tarefa);
        Task RemoveTarefaAsync(int id);
        Task AddTaskHistoryAsync(TarefaHistorico historico);
    }
}
