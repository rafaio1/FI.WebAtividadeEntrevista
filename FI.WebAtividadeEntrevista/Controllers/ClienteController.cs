using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;
using WebAtividadeEntrevista.Service;


namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        private readonly BeneficiariosCacheService _beneficiariosCacheService = new BeneficiariosCacheService();


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Incluir()
        {
            ViewBag.Guid = Guid.NewGuid().ToString();

            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model, string guid)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBene = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                var beneficiarios = _beneficiariosCacheService.Obter(guid);

                model.Id = bo.Incluir(new Cliente(model.CPF, model.Email, model.Telefone, model.CEP)
                {
                    Cidade = model.Cidade,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                });

                if (beneficiarios != null && beneficiarios.Count() > 0)
                {
                    boBene.SalvarLista(beneficiarios, model.Id);
                }


                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model, string guid)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiario boBene = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                var beneficiarios = _beneficiariosCacheService.Obter(guid);

                bo.Alterar(new Cliente(model.CPF, model.Email, model.Telefone, model.CEP)
                {
                    Id = model.Id,
                    Cidade = model.Cidade,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                });

                if (beneficiarios != null && beneficiarios.Count() > 0)
                {
                    boBene.SalvarLista(beneficiarios, model.Id);
                }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            ViewBag.Guid = Guid.NewGuid().ToString();

            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF
                };


            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}