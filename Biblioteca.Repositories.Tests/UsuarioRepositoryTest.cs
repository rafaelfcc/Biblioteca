using Biblioteca.Data.Context;
using Biblioteca.Data.Repositories;
using Biblioteca.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Repositories.Tests
{
    public class UsuarioRepositoryTest : IDisposable
    {
        #region Core da Classe de Testes
        private readonly DataContext _context;
        private readonly UsuarioRepository _repository;

        public UsuarioRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);
            _repository = new UsuarioRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        #endregion

        #region Métodos de Teste
        [Fact]
        public void Insert_ShouldAddUsuarioToDatabase()
        {
            var usuario = new Usuario
            {
                Nome = "Maria",
                DataNascimento = new DateTime(1990, 5, 12),
                EmailLogin = "maria@email.com",
                Senha = "senha123"
            };

            var insertedId = _repository.Insert(usuario);

            var usuarioNoBanco = _context.Set<Usuario>().Find(insertedId);
            Assert.NotNull(usuarioNoBanco);
            Assert.Equal("Maria", usuarioNoBanco.Nome);
            Assert.Equal("maria@email.com", usuarioNoBanco.EmailLogin);
        }

        [Fact]
        public void Get_ShouldReturnUsuarioById()
        {
            var usuario = new Usuario
            {
                EmailLogin = "joao@email.com",
                Nome = "João",
                DataNascimento = new DateTime(1985, 1, 20),
                Senha = "abc123"
            };
            _context.Set<Usuario>().Add(usuario);
            _context.SaveChanges();

            var usuarioRetornado = _repository.Get(usuario.EmailLogin);

            Assert.NotNull(usuarioRetornado);
            Assert.Equal("João", usuarioRetornado.Nome);
            Assert.Equal(usuario.EmailLogin, usuarioRetornado.EmailLogin);
        }

        [Fact]
        public void Update_ShouldUpdateUsuarioData()
        {
            var usuario = new Usuario
            {
                EmailLogin = "ana@email.com",
                Nome = "Ana",
                DataNascimento = new DateTime(2000, 3, 10),
                Senha = "senhaantiga"
            };
            _context.Set<Usuario>().Add(usuario);
            _context.SaveChanges();
            _context.Entry(usuario).State = EntityState.Detached;

            var usuarioAtualizado = new Usuario
            {
                EmailLogin = "ana@email.com",
                Nome = "Ana Clara",
                DataNascimento = new DateTime(2000, 3, 10),
                Senha = "senhanova"
            };

            var result = _repository.Update(usuarioAtualizado);

            var usuarioNoBanco = _context.Set<Usuario>().Find("ana@email.com");
            Assert.True(result);
            Assert.Equal("Ana Clara", usuarioNoBanco.Nome);
            Assert.Equal("senhanova", usuarioNoBanco.Senha);
        }

        [Fact]
        public void Delete_ShouldRemoveUsuarioFromDatabase()
        {
            var usuario = new Usuario
            {
                EmailLogin = "lucas@email.com",
                Nome = "Lucas",
                DataNascimento = new DateTime(1999, 8, 25),
                Senha = "teste"
            };
            _context.Set<Usuario>().Add(usuario);
            _context.SaveChanges();

            var result = _repository.Delete("lucas@email.com");

            var usuarioNoBanco = _context.Set<Usuario>().Find("lucas@email.com");
            Assert.True(result);
            Assert.Null(usuarioNoBanco);
        }

        [Fact]
        public void GetList_ShouldReturnFilteredUsuarios()
        {
            _context.Set<Usuario>().AddRange(
                new Usuario { EmailLogin = "a@email.com", Nome = "A", DataNascimento = DateTime.Today, Senha = "123" },
                new Usuario { EmailLogin = "b@email.com", Nome = "B", DataNascimento = DateTime.Today, Senha = "123" },
                new Usuario { EmailLogin = "c@email.com", Nome = "A", DataNascimento = DateTime.Today, Senha = "123" }
            );
            _context.SaveChanges();

            var lista = _repository.GetList(u => u.Nome == "A");
            Assert.Equal(2, lista.Count);
        }
        #endregion
    }
}
