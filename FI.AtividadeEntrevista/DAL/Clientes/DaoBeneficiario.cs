using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

// Possível otimização usando o entity framework afim de ganhar processamento com metódos CRUD básico
namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Beneficiario
    /// </summary>
    internal class DaoBeneficiario : AcessoDados
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        internal long Incluir(DML.Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCliente", beneficiario.IDCliente));

            DataSet ds = base.Consultar("FI_SP_IncBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Consulta o Beneficiario
        /// </summary>
        /// <param name="Id">Identificador do Beneficiario</param>
        internal List<DML.Beneficiario> Consultar(long IdCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("IdCliente", IdCliente));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<DML.Beneficiario> bene = Converter(ds);

            return bene;
        }

        /// <summary>
        /// Lista todos os beneficiarios
        /// </summary>
        internal List<DML.Beneficiario> Listar()
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", 0));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<DML.Beneficiario> bene = Converter(ds);

            return bene;
        }

        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        internal void Alterar(DML.Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", beneficiario.Id));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCliente", beneficiario.IDCliente));

            base.Executar("FI_SP_AltBeneficiario", parametros);
        }


        /// <summary>
        /// Excluir Beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        internal void Excluir(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            base.Executar("FI_SP_DelBeneficiario", parametros);
        }

        private List<DML.Beneficiario> Converter(DataSet ds)
        {
            List<DML.Beneficiario> lista = new List<DML.Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Beneficiario bene = new DML.Beneficiario();
                    bene.Id = row.Field<long>("Id");
                    bene.Nome = row.Field<string>("Nome");
                    bene.CPF = row.Field<string>("CPF");
                    bene.IDCliente = row.Field<long>("IDCliente");
                    lista.Add(bene);
                }
            }

            return lista;
        }


        internal List<Beneficiario> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("iniciarEm", iniciarEm));
            parametros.Add(new System.Data.SqlClient.SqlParameter("quantidade", quantidade));
            parametros.Add(new System.Data.SqlClient.SqlParameter("campoOrdenacao", campoOrdenacao));
            parametros.Add(new System.Data.SqlClient.SqlParameter("crescente", crescente));

            DataSet ds = base.Consultar("FI_SP_PesqBeneficiario", parametros);
            List<DML.Beneficiario> bene = Converter(ds);

            int iQtd = 0;

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out iQtd);

            qtd = iQtd;

            return bene;
        }

        /// <summary>
        /// Adiciona o beneficiário ou Atualiza caso ele já exista
        /// </summary>
        /// <param name="beneficiarios"></param>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        internal List<Beneficiario> SalvarLista(List<Beneficiario> beneficiarios, long IdCliente)
        {
            var beneficiariosDb = Consultar(IdCliente);
            var listaSalvos = new List<Beneficiario>();

            // Seleciono somente o cpf como uma HashSet para otimizar a inserção, Atualização e Deleção dos registros
            var cpfsRecebidos = beneficiarios.Select(b => b.CPF).ToHashSet();

            // Percorro a lista para validar os beneficiários que serão inseridos ou atualizados
            foreach (var bene in beneficiarios)
            {
                var beneficiarioBd = beneficiariosDb.FirstOrDefault(b => b.CPF == bene.CPF);

                if (beneficiarioBd != null)
                {
                    beneficiarioBd.Nome = bene.Nome;

                    Alterar(beneficiarioBd);
                    listaSalvos.Add(beneficiarioBd);
                }
                else
                {
                    bene.IDCliente = IdCliente;
                    bene.Id = Incluir(bene);

                    listaSalvos.Add(bene);
                }
            }

            // Vejo o que está para ser deletado
            var paraDeletar = beneficiariosDb.Where(b => !cpfsRecebidos.Contains(b.CPF)).ToList();

            // Removo todos que devem ser deletados
            foreach (var bene in paraDeletar)
            {
                Excluir(bene.Id);
            }

            return listaSalvos;
        }
    }
}
