using GerenciadorTarefasAPI.Data;
using GerenciadorTarefasAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorTarefasAPI.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly ApplicationDbContext _context;

        public TarefaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarefa>> GetTarefasByProjetoIdAsync(int projetoId)
        {
            return await _context.Tarefas.Where(t => t.ProjetoId == projetoId).ToListAsync();
        }

        public async Task<Tarefa> GetTarefaByIdAsync(int id)
        {
            return await _context.Tarefas.FindAsync(id);
        }

        public async Task AddTarefaAsync(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTarefaAsync(Tarefa tarefa)
        {
            _context.Entry(tarefa).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTarefaAsync(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddTaskHistoryAsync(TarefaHistorico historico)
        {
            _context.TarefaHistoricos.Add(historico);
            await _context.SaveChangesAsync();
        }
    }
}
