using Microsoft.EntityFrameworkCore;
using GerenciadorTarefasAPI.Models;

namespace GerenciadorTarefasAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<TarefaHistorico> TarefaHistoricos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarefa>()
                .HasOne(t => t.Projeto)
                .WithMany(p => p.Tarefas)
                .HasForeignKey(t => t.ProjetoId);

            modelBuilder.Entity<TarefaHistorico>()
                .HasOne(h => h.Tarefa)
                .WithMany(t => t.Historicos)
                .HasForeignKey(h => h.TarefaId);
        }
    }
}
