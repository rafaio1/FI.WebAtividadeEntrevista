using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            return bene.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            bene.Alterar(beneficiario);
        }

        /// <summary>
        /// Excluir o beneficiário pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            bene.Excluir(id);
        }

        /// <summary>
        /// Lista os Beneficiários
        /// </summary>
        public List<DML.Beneficiario> Listar()
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            return bene.Listar();
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="idCliente">id do cliente</param>
        /// <returns></returns>
        public List<DML.Beneficiario> Consultar(long idCliente)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            return bene.Consultar(idCliente);
        }

        /// <summary>
        /// Lista os Beneficiários
        /// </summary>
        public List<DML.Beneficiario> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            return bene.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// Adiciona uma lista de Beneficiários
        /// </summary>
        /// <param name="beneficiarios"></param>
        /// <param name="idCliente"></param>
        public List<Beneficiario> SalvarLista(List<Beneficiario> beneficiarios, long idCliente)
        {
            DAL.DaoBeneficiario bene = new DAL.DaoBeneficiario();
            return bene.SalvarLista(beneficiarios, idCliente);
        }
    }
}
