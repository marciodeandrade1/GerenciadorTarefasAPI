using System.Collections.Generic;
using System.Threading.Tasks;
using GerenciadorTarefasAPI.Data;
using GerenciadorTarefasAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorTarefasAPI.Repositories
{
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjetoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Projeto>> GetProjetosAsync()
        {
            return await _context.Projetos.Include(p => p.Tarefas).ToListAsync();
        }

        public async Task<Projeto> GetProjetoByIdAsync(int id)
        {
            return await _context.Projetos.Include(p => p.Tarefas).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddProjetoAsync(Projeto projeto)
        {
            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjetoAsync(Projeto projeto)
        {
            _context.Entry(projeto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveProjetoAsync(int id)
        {
            var projeto = await _context.Projetos.FindAsync(id);
            if (projeto != null)
            {
                _context.Projetos.Remove(projeto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
