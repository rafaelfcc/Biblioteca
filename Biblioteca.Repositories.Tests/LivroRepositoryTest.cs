using Biblioteca.Data.Context;
using Biblioteca.Domain.Entities;
using Livraria.Data.Repositories;
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
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _context = new DataContext(options);
            _repository = new LivroRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        #endregion

        #region Métodos de Teste
        [Fact]
        public void Insert_ShouldAddLivroToDatabase()
        {
            // Arrange
            var novoLivro = new Livro { Titulo = "O Pequeno Príncipe" };

            // Act
            _repository.Insert(novoLivro);

            // Assert
            var livroNoBanco = _context.Set<Livro>().Find(novoLivro.Id);
            Assert.NotNull(livroNoBanco);
            Assert.Equal(novoLivro.Titulo, livroNoBanco.Titulo);
        }

        [Fact]
        public void Get_ShouldReturnLivroById()
        {
            // Arrange
            var livroId = Guid.NewGuid();
            var livroSeed = new Livro { Id = livroId, Titulo = "Senhor dos Anéis" };
            _context.Set<Livro>().Add(livroSeed);
            _context.SaveChanges();

            // Act
            var livroRetornado = _repository.Get(livroId);

            // Assert
            Assert.NotNull(livroRetornado);
            Assert.Equal(livroId, livroRetornado.Id);
            Assert.Equal("Senhor dos Anéis", livroRetornado.Titulo);
        }

        [Fact]
        public void Update_ShouldUpdateLivroData()
        {
            // Arrange
            var livroId = Guid.NewGuid();
            var livroSeed = new Livro { Id = livroId, Titulo = "Título Antigo" };
            _context.Set<Livro>().Add(livroSeed);
            _context.SaveChanges();
            _context.Entry(livroSeed).State = EntityState.Detached;

            var livroParaAtualizar = new Livro { Id = livroId, Titulo = "Título Novo" };

            // Act
            var result = _repository.Update(livroParaAtualizar);

            // Assert
            var livroNoBanco = _context.Set<Livro>().Find(livroId);
            Assert.True(result);
            Assert.Equal("Título Novo", livroNoBanco.Titulo);
        }

        [Fact]
        public void Delete_ShouldRemoveLivroFromDatabase()
        {
            // Arrange
            var livroId = Guid.NewGuid();
            var livroSeed = new Livro { Id = livroId, Titulo = "Livro para Excluir" };
            _context.Set<Livro>().Add(livroSeed);
            _context.SaveChanges();

            // Act
            var result = _repository.Delete(livroId);

            // Assert
            var livroNoBanco = _context.Set<Livro>().Find(livroId);
            Assert.True(result);
            Assert.Null(livroNoBanco);
        }

        [Fact]
        public void GetList_ShouldReturnFilteredListByTitulo()
        {
            // Arrange
            _context.Set<Livro>().AddRange(
                new Livro { Id = Guid.NewGuid(), Titulo = "A" },
                new Livro { Id = Guid.NewGuid(), Titulo = "B" },
                new Livro { Id = Guid.NewGuid(), Titulo = "A" }
            );
            _context.SaveChanges();

            // Act
            var listaA = _repository.GetList(l => l.Titulo == "A");

            // Assert
            Assert.Equal(2, listaA.Count);
        }
        #endregion
    }
}
