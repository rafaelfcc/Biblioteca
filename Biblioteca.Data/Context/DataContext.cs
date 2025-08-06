using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteca.Data.Context
{
    public class DataContext : DbContext
    {
        #region Conteudo Principal
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // DbSets para as novas tabelas
        public DbSet<Usuario> ContasAcesso { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<GeneroLivro> GeneroLivros { get; set; }
        public DbSet<Editora> Editoras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UsuarioModelCreating(modelBuilder);
            LivroModelCreating(modelBuilder);
            GeneroLivroModelCreating(modelBuilder);
            EditoraModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region Metodos Auxiliares por Entidade
        private void UsuarioModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");
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
                entity.Property(l => l.Autor).IsRequired().HasMaxLength(255);
                entity.Property(l => l.Sinopse).HasMaxLength(5000);
                entity.Property(l => l.UsuarioRegistro).IsRequired().HasMaxLength(255);
                entity.Property(l => l.CaminhoFoto).HasMaxLength(500);
                entity.Property(l => l.FotoLivro).IsRequired();

                // Configuração da chave estrangeira para GeneroLivro
                entity.HasOne(l => l.GeneroLivro)
                      .WithMany()
                      .HasForeignKey(l => l.GeneroId);

                // Configuração da chave estrangeira para Editora
                entity.HasOne(l => l.Editora)
                      .WithMany()
                      .HasForeignKey(l => l.EditoraId);

                // Seed de dados para a tabela Livros
                entity.HasData(
                    new Livro { Id = Guid.NewGuid(), Titulo = "O Pequeno Príncipe", ISBN = "978-85-359-0277-7", GeneroId = 1, Autor = "Antoine de Saint-Exupéry", EditoraId = 1, Sinopse = "Uma história poética e filosófica contada por um piloto que encontra um pequeno príncipe vindo de outro planeta. A obra aborda temas como amizade, amor e o sentido da vida.", UsuarioRegistro="rfccrj@gmail.com", CaminhoFoto = "0", FotoLivro = 0 },
                    new Livro { Id = Guid.NewGuid(), Titulo = "O Senhor dos Anéis", ISBN = "978-85-222-0573-5", GeneroId = 2, Autor = "J.R.R. Tolkien", EditoraId = 2, Sinopse = "Uma épica jornada pela Terra Média em que um grupo improvável de heróis precisa destruir um anel mágico e maligno para evitar que um senhor das trevas domine o mundo.", UsuarioRegistro = "rfccrj@gmail.com", CaminhoFoto = "0", FotoLivro = 0 },
                    new Livro { Id = Guid.NewGuid(), Titulo = "Teoria da Música", ISBN = "978-85-7060-013-1", GeneroId = 3, Autor = "Bohumil Med", EditoraId = 3, Sinopse = "Obra consagrada que apresenta fundamentos teóricos da música com linguagem acessível e progressiva. Utilizada em escolas de música, conservatórios e cursos técnicos.", UsuarioRegistro = "rfccrj@gmail.com", CaminhoFoto = "/uploads/2dfbf92a-77de-471d-3282-08ddd1ea4bf9_teoria_da_musica.JPG", FotoLivro = 0 },
                    new Livro { Id = Guid.NewGuid(), Titulo = "Ansiedade - Como enfrentar o mal do século", ISBN = "978-85-452-0016-2", GeneroId = 4, Autor = "Augusto Cury", EditoraId = 4, Sinopse = "O livro apresenta conceitos da psicologia para ajudar o leitor a entender e enfrentar a ansiedade, com foco na Síndrome do Pensamento Acelerado, termo cunhado pelo autor.", UsuarioRegistro = "rfccrj@gmail.com", CaminhoFoto = "0", FotoLivro = 0 }
                );
            });
        }

        private void GeneroLivroModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeneroLivro>(entity =>
            {
                entity.ToTable("GeneroLivro");
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Id).ValueGeneratedNever(); // Para que não seja gerado automaticamente, já que os IDs são fixos
                entity.Property(g => g.Genero).IsRequired().HasMaxLength(30);

                // Seed de dados para a tabela GeneroLivro
                entity.HasData(
                    new GeneroLivro { Id = 1, Genero = "Fábula" },
                    new GeneroLivro { Id = 2, Genero = "Fantasia" },
                    new GeneroLivro { Id = 3, Genero = "Didático" },
                    new GeneroLivro { Id = 4, Genero = "Auto-Ajuda" },
                    new GeneroLivro { Id = 5, Genero = "Romance" },
                    new GeneroLivro { Id = 6, Genero = "Tecnologia" },
                    new GeneroLivro { Id = 7, Genero = "Ciência" },
                    new GeneroLivro { Id = 8, Genero = "Saúde e Bem-Estar" },
                    new GeneroLivro { Id = 9, Genero = "Ciências Sociais" }
                );
            });
        }

        private void EditoraModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Editora>(entity =>
            {
                entity.ToTable("Editora");
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Id).ValueGeneratedNever(); // Para que não seja gerado automaticamente, já que os IDs são fixos
                entity.Property(g => g.Nome).IsRequired().HasMaxLength(30);

                // Seed de dados para a tabela Editora
                entity.HasData(
                    new Editora { Id = 1, Nome = "Agir" },
                    new Editora { Id = 2, Nome = "Martins Fontes" },
                    new Editora { Id = 3, Nome = "Ricordi" },
                    new Editora { Id = 4, Nome = "Benvirá" }
                );
            });
        }
        #endregion
    }

}