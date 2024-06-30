using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorTarefasAPI.Models;
using GerenciadorTarefasAPI.Repositories;

namespace GerenciadorTarefasAPI.Services
{
    public class TarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaService(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<IEnumerable<Tarefa>> GetTarefasByProjetoIdAsync(int projetoId)
        {
            return await _tarefaRepository.GetTarefasByProjetoIdAsync(projetoId);
        }

        public async Task CreateTarefaAsync(int projetoId, Tarefa tarefa)
        {
            if ((await _tarefaRepository.GetTarefasByProjetoIdAsync(projetoId)).Count() >= 20)
            {
                throw new InvalidOperationException("Não pode adicionar mais tarefas neste projeto.");
            }

            tarefa.CriadoEm = DateTime.UtcNow;
            tarefa.ProjetoId = projetoId;
            await _tarefaRepository.AddTarefaAsync(tarefa);
        }

        public async Task UpdateTarefaAsync(int tarefaId, Tarefa tarefa)
        {
            var tarefaExistente = await _tarefaRepository.GetTarefaByIdAsync(tarefaId);
            if (tarefaExistente == null)
            {
                throw new InvalidOperationException("Tarefa não encontrada.");
            }

            if (tarefaExistente.Prioridade != tarefa.Prioridade)
            {
                throw new InvalidOperationException("Não é permitido alterar a prioridade da tarefa.");
            }

            tarefaExistente.Descricao = tarefa.Descricao;
            tarefaExistente.Status = tarefa.Status;
            tarefaExistente.AtualizadoEm = DateTime.UtcNow;

            var historico = new TarefaHistorico
            {
                TarefaId = tarefaId,
                AlteraDescricao = "Tarefa atualizada",
                AlteraDate = DateTime.UtcNow,
                AlteradoPor = "Usuário" // Deve ser substituído pelo usuário real em um aplicativo real
            };

            await _tarefaRepository.AddTaskHistoryAsync(historico);
            await _tarefaRepository.UpdateTarefaAsync(tarefaExistente);
        }

        public async Task DeleteTarefaAsync(int tarefaId)
        {
            await _tarefaRepository.RemoveTarefaAsync(tarefaId);
        }
    }
}
