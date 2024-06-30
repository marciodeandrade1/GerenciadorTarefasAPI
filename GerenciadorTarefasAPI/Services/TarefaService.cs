using AutoMapper;
using GerenciadorTarefasAPI.DTOs;
using GerenciadorTarefasAPI.Models;
using GerenciadorTarefasAPI.Repositories;

namespace GerenciadorTarefasAPI.Services
{
    public class TarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IMapper _mapper;

        public TarefaService(ITarefaRepository tarefaRepository, IMapper mapper)
        {
            _tarefaRepository = tarefaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TarefaDTO>> GetTarefasByProjetoIdAsync(int projetoId)
        {
            var tarefas = await _tarefaRepository.GetTarefasByProjetoIdAsync(projetoId);
            return _mapper.Map<IEnumerable<TarefaDTO>>(tarefas);
        }

        public async Task CreateTarefaAsync(int projetoId, TarefaDTO tarefaDTO)
        {
            if ((await _tarefaRepository.GetTarefasByProjetoIdAsync(projetoId)).Count() >= 20)
            {
                throw new InvalidOperationException("Não pode adicionar mais tarefas neste projeto.");
            }

            var tarefa = _mapper.Map<Tarefa>(tarefaDTO);
            tarefa.CriadoEm = DateTime.UtcNow;
            tarefa.ProjetoId = projetoId;
            await _tarefaRepository.AddTarefaAsync(tarefa);
        }

        public async Task UpdateTarefaAsync(int tarefaId, TarefaDTO tarefaDTO)
        {
            var tarefaExistente = await _tarefaRepository.GetTarefaByIdAsync(tarefaId);
            if (tarefaExistente == null)
            {
                throw new InvalidOperationException("Tarefa não encontrada.");
            }

            if (tarefaExistente.Prioridade != tarefaDTO.Prioridade)
            {
                throw new InvalidOperationException("Não é permitido alterar a prioridade da tarefa.");
            }

            tarefaExistente.Descricao = tarefaDTO.Descricao;
            tarefaExistente.Status = tarefaDTO.Status;
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
