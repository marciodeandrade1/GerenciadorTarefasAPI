using AutoMapper;
using GerenciadorTarefasAPI.DTOs;
using GerenciadorTarefasAPI.Models;
using GerenciadorTarefasAPI.Repositories;

namespace GerenciadorTarefasAPI.Services
{
    public class ProjetoService
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IMapper _mapper;

        public ProjetoService(IProjetoRepository projetoRepository, IMapper mapper)
        {
            _projetoRepository = projetoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjetoDTO>> GetProjetosAsync()
        {
            var projetos = await _projetoRepository.GetProjetosAsync();
            return _mapper.Map<IEnumerable<ProjetoDTO>>(projetos);
        }

        public async Task CreateProjetoAsync(ProjetoDTO projetoDTO)
        {
            var projeto = _mapper.Map<Projeto>(projetoDTO);
            await _projetoRepository.AddProjetoAsync(projeto);
        }

        public async Task RemoveProjetoAsync(int id)
        {
            await _projetoRepository.RemoveProjetoAsync(id);
        }
    }
}
