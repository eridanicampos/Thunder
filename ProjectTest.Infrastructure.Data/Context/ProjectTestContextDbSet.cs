using Microsoft.EntityFrameworkCore;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Infrastructure.Data.Context
{
    public partial class ProjectTestContext
    {
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<AcessoUsuario> Acessos { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<EnderecoEntrega> EnderecosEntrega { get; set; }
        public virtual DbSet<Tarefa> Tarefas { get; set; }


    }
}
