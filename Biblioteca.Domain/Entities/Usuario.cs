using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Domain.Entities
{
    public class Usuario
    {
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; } = DateTime.MinValue;
        public string EmailLogin { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;

        public Usuario()
        {
                
        }

        public Usuario(string nome, DateTime dataNascimento, string emailLogin, string senha)
        {
            Nome = nome;
            DataNascimento = dataNascimento;
            EmailLogin = emailLogin;
            Senha = senha;
        }
    }
}
