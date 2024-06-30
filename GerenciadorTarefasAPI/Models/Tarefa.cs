namespace GerenciadorTarefasAPI.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public string Prioridade { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; }
        public ICollection<TarefaHistorico> Historicos { get; set; }
    }
}
