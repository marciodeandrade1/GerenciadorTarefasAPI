using System.Collections.Generic;
using System.Threading.Tasks;
using GerenciadorTarefasAPI.Models;
using GerenciadorTarefasAPI.Repositories;

namespace GerenciadorTarefasAPI.Services
{
    public class ProjetoService
    {
        private readonly IProjetoRepository _projetoRepository;

        public ProjetoService(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task<IEnumerable<Projeto>> GetProjetosAsync()
        {
            return await _projetoRepository.GetProjetosAsync();
        }

        public async Task<Projeto> GetProjetoByIdAsync(int id)
        {
            return await _projetoRepository.GetProjetoByIdAsync(id);
        }

        public async Task CreateProjetoAsync(Projeto projeto)
        {
            await _projetoRepository.AddProjetoAsync(projeto);
        }

        public async Task RemoveProjetoAsync(int id)
        {
            var projeto = await _projetoRepository.GetProjetoByIdAsync(id);
            if (projeto == null)
            {
                throw new InvalidOperationException("Projeto não encontrado.");
            }

            if (projeto.Tarefas.Any(t => t.Status != "Concluída"))
            {
                throw new InvalidOperationException("Não é possível remover um projeto com tarefas pendentes.");
            }

            await _projetoRepository.RemoveProjetoAsync(id);
        }
    }
}
