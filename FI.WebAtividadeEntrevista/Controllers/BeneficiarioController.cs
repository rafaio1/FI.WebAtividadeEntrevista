using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebAtividadeEntrevista.Service;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        private readonly BeneficiariosCacheService _beneficiariosCacheService = new BeneficiariosCacheService();


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Incluir(string guid)
        {
            ViewBag.Guid = guid;
            _beneficiariosCacheService.Criar(guid, new List<Beneficiario>());

            return PartialView("Salvar");
        }

        [HttpPost]
        public JsonResult Incluir(BeneficiarioModel model, string guid)
        {
            var beneficiarios = new Beneficiario(model.Id, model.Nome, model.CPF, model.IDCliente);

            _beneficiariosCacheService.Salvar(guid, beneficiarios);

            return Json("Cadastro efetuado com sucesso");
        }

        [HttpPost]
        public JsonResult Alterar(BeneficiarioModel model, string guid)
        {
            var beneficiarios = new Beneficiario(model.Id, model.Nome, model.CPF, model.IDCliente);

            _beneficiariosCacheService.Salvar(guid, beneficiarios);

            return Json("Alteração efetuada com sucesso");
        }

        [HttpPost]
        public JsonResult Excluir(BeneficiarioModel model, string guid)
        {
            var beneficiarios = new Beneficiario(model.Id, model.Nome, model.CPF, model.IDCliente);

            _beneficiariosCacheService.Remover(guid, beneficiarios);

            return Json("Exclusão efetuado com sucesso");
        }

        [HttpGet]
        public ActionResult Alterar(long id, string guid)
        {
            BoBeneficiario bo = new BoBeneficiario();
            List<Beneficiario> Beneficiarios = bo.Consultar(id);
            List<Models.BeneficiarioModel> model = new List<Models.BeneficiarioModel>();

            if (Beneficiarios != null)
            {
                model = Beneficiarios.Select(bene => new BeneficiarioModel()
                {
                    Id = bene.Id,
                    Nome = bene.Nome,
                    CPF = bene.CPF,
                    IDCliente = bene.IDCliente
                }).ToList();

                _beneficiariosCacheService.Criar(guid, Beneficiarios);
            }
            else
            {
                _beneficiariosCacheService.Criar(guid, new List<Beneficiario>());
            }

            ViewBag.Guid = guid;

            return PartialView("Salvar");
        }

        [HttpPost]
        public JsonResult BeneficiarioList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null, string guid = null)
        {
            if (string.IsNullOrEmpty(guid))
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

                    List<Beneficiario> Beneficiarios = new BoBeneficiario().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                    Beneficiarios = Beneficiarios.Select(x => new Beneficiario
                    {
                        Id = x.Id,
                        IDCliente = x.IDCliente,
                        Nome = x.Nome,
                        CPF = ulong.TryParse(new string(x.CPF.Where(char.IsDigit).ToArray()), out var n) ? n.ToString(@"000\.000\.000\-00") : x.CPF,
                    }).ToList();

                    //Return result to jTable
                    return Json(new { Result = "OK", Records = Beneficiarios, TotalRecordCount = qtd });
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "ERROR", Message = ex.Message });
                }
            }
            else
            {
                var beneficiarios = _beneficiariosCacheService.Obter(guid);
                beneficiarios = beneficiarios.Select(x => new Beneficiario
                {
                    Id = x.Id,
                    IDCliente = x.IDCliente,
                    Nome = x.Nome,
                    CPF = ulong.TryParse(new string(x.CPF.Where(char.IsDigit).ToArray()), out var n) ? n.ToString(@"000\.000\.000\-00") : x.CPF,
                }).ToList();

                return Json(new { Result = "OK", Records = beneficiarios, TotalRecordCount = beneficiarios.Count() });
            }
        }
    }
}