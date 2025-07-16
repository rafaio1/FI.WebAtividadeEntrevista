using FI.AtividadeEntrevista.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DML
{
    /// <summary>
    /// Classe de cliente que representa o registo na tabela Cliente do Banco de Dados
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// CEP
        /// </summary>
        public string CEP { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        public string Cidade { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        public string Logradouro { get; set; }

        /// <summary>
        /// Nacionalidade
        /// </summary>
        public string Nacionalidade { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        public string Sobrenome { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// CPF - Comprovante de Pessoa Física
        /// </summary>
        public string CPF { get; set; }

        public Cliente() { }

        public Cliente(string cpf, string email, string telefone, string cep)
        {
            // Validação do CPF
            var cpfDigitos = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpfDigitos.Length != 11)
                throw new Exception("CPF deve conter 11 dígitos numéricos.");

            if (!ValidarCPF(cpfDigitos))
                throw new Exception("CPF inválido.");

            // Válida Email e formato do mesmo
            if (string.IsNullOrWhiteSpace(email) ||
                !Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
                throw new Exception("E-mail inválido.");

            // Válida a quantidade de dígitos para o telefone
            var telefoneNumbers = new string(telefone.Where(char.IsDigit).ToArray());
            if (telefoneNumbers.Length < 10 || telefoneNumbers.Length > 11)
                throw new Exception("Telefone inválido. Deve ter 10 ou 11 dígitos.");

            // Validação de CEP
            var cepNumbers = new string(cep.Where(char.IsDigit).ToArray());
            if (cepNumbers.Length != 8)
                throw new Exception("CEP inválido. Deve conter 8 dígitos.");

            // Atribuições
            CPF = cpfDigitos;
            Email = email.Trim();
            Telefone = telefoneNumbers;
            CEP = cepNumbers;
        }

        private bool ValidarCPF(string cpf)
        {
            // Números todos iguais não são válidos (ex: 00000000000, 11111111111, ...)
            if (new string(cpf[0], 11) == cpf)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            // Cálculo do primeiro dígito verificador
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = (resto < 2) ? 0 : 11 - resto;

            // Cálculo do segundo dígito verificador
            tempCpf += digito1;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digito2 = (resto < 2) ? 0 : 11 - resto;

            // Verifica se os dígitos calculados conferem com os informados
            string digitosInformados = cpf.Substring(9, 2);
            string digitosCalculados = $"{digito1}{digito2}";

            return digitosInformados == digitosCalculados;
        }
    }    
}
