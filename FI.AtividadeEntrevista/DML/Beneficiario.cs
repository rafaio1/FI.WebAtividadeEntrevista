using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DML
{
    /// <summary>
    /// Classe de beneficiário que representa o registo na tabela Beneficiarios do Banco de Dados
    /// </summary>
    public class Beneficiario
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// CPF - Comprovante de Pessoa Física
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Id do Cliente
        /// </summary>
        public long IDCliente { get; set; }

        public Beneficiario() { }

        public Beneficiario(long id, string nome, string cpf, long idCliente)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("Nome não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(cpf))
                throw new Exception("CPF não pode ser vazio.");

            var cpfDigitos = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpfDigitos.Length != 11)
                throw new Exception("CPF deve conter 11 dígitos numéricos.");

            if (!ValidarCPF(cpfDigitos))
                throw new Exception("CPF inválido.");

            if (idCliente <= 0)
                throw new Exception("Cliente inválido.");

            Id = id;
            Nome = nome;
            CPF = cpfDigitos;
            IDCliente = idCliente;
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
