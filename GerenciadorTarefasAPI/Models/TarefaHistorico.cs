namespace GerenciadorTarefasAPI.Models
{
    public class TarefaHistorico
    {
        public int Id { get; set; }
        public int TarefaId { get; set; }
        public Tarefa Tarefa { get; set; }
        public string AlteraDescricao { get; set; }
        public DateTime AlteraDate { get; set; }
        public string AlteradoPor { get; set; }
    }
}
