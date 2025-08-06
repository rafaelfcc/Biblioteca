using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Biblioteca.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Repositories.Tests
{
    public class LivroRepositoryTest : IDisposable
    {
        #region Core da Classe de Testes
        private readonly DataContext _context;
        private readonly LivroRepository _repository;

        public LivroRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);
            _repository = new LivroRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private (int generoId, int editoraId) SeedGeneroEditora()
        {
            var genero = new GeneroLivro { Id = 1, Genero = "Ficção" };
            var editora = new Editora { Id = 1, Nome = "Companhia das Letras" };

            _context.Add(genero);
            _context.Add(editora);
            _context.SaveChanges();

            return (genero.Id, editora.Id);
        }
        #endregion

        #region Métodos de Teste
        [Fact]
        public void Insert_ShouldAddLivroToDatabase()
        {
            var (generoId, editoraId) = SeedGeneroEditora();

            var novoLivro = new Livro
            {
                Titulo = "O Pequeno Príncipe",
                GeneroId = generoId,
                EditoraId = editoraId
            };

            var insertedId = _repository.Insert(novoLivro);

            var livroNoBanco = _context.Set<Livro>().Find(insertedId);
            Assert.NotNull(livroNoBanco);
            Assert.Equal(novoLivro.Titulo, livroNoBanco.Titulo);
        }

        [Fact]
        public void Get_ShouldReturnLivroById()
        {
            var (generoId, editoraId) = SeedGeneroEditora();

            var livroId = Guid.NewGuid();
            var livroSeed = new Livro
            {
                Id = livroId,
                Titulo = "Senhor dos Anéis",
                GeneroId = generoId,
                EditoraId = editoraId,
                GeneroLivro = _context.GeneroLivros.Find(generoId)!,
                Editora = _context.Editoras.Find(editoraId)!
            };
            _context.Set<Livro>().Add(livroSeed);
            _context.SaveChanges();

            var livroRetornado = _repository.Get(livroId);

            Assert.NotNull(livroRetornado);
            Assert.Equal(livroId, livroRetornado.Id);
            Assert.Equal("Senhor dos Anéis", livroRetornado.Titulo);
            Assert.NotNull(livroRetornado.Editora);
            Assert.NotNull(livroRetornado.GeneroLivro);
        }

        [Fact]
        public void Update_ShouldUpdateLivroData()
        {
            var (generoId, editoraId) = SeedGeneroEditora();

            var livroId = Guid.NewGuid();
            var livroSeed = new Livro
            {
                Id = livroId,
                Titulo = "Título Antigo",
                GeneroId = generoId,
                EditoraId = editoraId,
                GeneroLivro = _context.GeneroLivros.Find(generoId)!,
                Editora = _context.Editoras.Find(editoraId)!
            };
            _context.Set<Livro>().Add(livroSeed);
            _context.SaveChanges();

            _context.Entry(livroSeed).State = EntityState.Detached;

            var livroParaAtualizar = new Livro
            {
                Id = livroId,
                Titulo = "Título Novo",
                GeneroId = generoId,
                EditoraId = editoraId
            };

            var result = _repository.Update(livroParaAtualizar);

            var livroNoBanco = _context.Set<Livro>().Find(livroId);
            Assert.True(result);
            Assert.Equal("Título Novo", livroNoBanco.Titulo);
        }

        [Fact]
        public void Delete_ShouldRemoveLivroFromDatabase()
        {
            var livroId = Guid.NewGuid();
            var livroSeed = new Livro { Id = livroId, Titulo = "Livro para Excluir" };
            _context.Set<Livro>().Add(livroSeed);
            _context.SaveChanges();

            var result = _repository.Delete(livroId);

            var livroNoBanco = _context.Set<Livro>().Find(livroId);
            Assert.True(result);
            Assert.Null(livroNoBanco);
        }

        [Fact]
        public void GetList_ShouldReturnFilteredListByTitulo()
        {
            _context.Set<Livro>().AddRange(
                new Livro { Id = Guid.NewGuid(), Titulo = "A" },
                new Livro { Id = Guid.NewGuid(), Titulo = "B" },
                new Livro { Id = Guid.NewGuid(), Titulo = "A" }
            );
            _context.SaveChanges();

            var listaA = _repository.GetList(l => l.Titulo == "A");

            Assert.Equal(2, listaA.Count);
        }
        #endregion
    }

}
