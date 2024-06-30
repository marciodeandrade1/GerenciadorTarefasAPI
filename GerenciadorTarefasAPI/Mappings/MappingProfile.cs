using AutoMapper;
using GerenciadorTarefasAPI.DTOs;
using GerenciadorTarefasAPI.Models;

namespace GerenciadorTarefasAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Projeto, ProjetoDTO>().ReverseMap();
            CreateMap<Tarefa, TarefaDTO>().ReverseMap();
            CreateMap<TarefaHistorico, TarefaHistoricoDTO>().ReverseMap();
        }
    }
}
