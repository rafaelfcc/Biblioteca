using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Data.Context
{
    public class DataContext : DbContext
    {
        #region Conteudo Principal
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Usuario> ContasAcesso { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UsuarioModelCreating(modelBuilder);
            LivroModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region Metodos Auxiliares por Entidade
        private void UsuarioModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("ContasAcesso");

                entity.HasKey(e => e.EmailLogin);

                entity.Property(e => e.Nome).IsRequired().HasMaxLength(255);

                entity.Property(e => e.DataNascimento).IsRequired();

                entity.Property(e => e.EmailLogin).IsRequired().HasMaxLength(255);

                entity.Property(e => e.Senha).IsRequired().HasMaxLength(255);
            });
        }

        private void LivroModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Livro>(entity =>
            {
                entity.ToTable("Livros");

                entity.HasKey(l => l.Id);
                entity.Property(l => l.Id).ValueGeneratedOnAdd();

                entity.Property(l => l.Titulo).IsRequired().HasMaxLength(255);

                entity.Property(l => l.ISBN).IsRequired().HasMaxLength(20);

                entity.Property(l => l.Genero).IsRequired().HasMaxLength(100);

                entity.Property(l => l.Autor).IsRequired().HasMaxLength(255);

                entity.Property(l => l.Editora).IsRequired().HasMaxLength(255);

                entity.Property(l => l.Sinopse).HasMaxLength(5000);

                entity.Property(l => l.CaminhoFoto).HasMaxLength(500);
            });
        }
        #endregion
    }
}