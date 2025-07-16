using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebAtividadeEntrevista.Service
{
    public class BeneficiariosCacheService
    {
        // Basicamente um crud de cache
        private readonly WebCacheService _cacheService = new WebCacheService();


        public void Criar(string guid, List<Beneficiario> beneficiarios)
        {
            if (beneficiarios == null)
            {
                beneficiarios = new List<Beneficiario>();
            }

            var lista = $"ListaBeneficiarios-{guid}";

            _cacheService.Salvar<List<Beneficiario>>(lista, beneficiarios, TimeSpan.FromMinutes(60));
        }

        public List<Beneficiario> Obter(string guid)
        {
            var lista = $"ListaBeneficiarios-{guid}";

            return _cacheService.Obter<List<Beneficiario>>(lista);
        }

        public void Salvar(string guid, Beneficiario model)
        {
            var lista = $"ListaBeneficiarios-{guid}";
            var beneficiarios = new List<Beneficiario>();

            beneficiarios = _cacheService.Obter<List<Beneficiario>>(lista);
            var beneficiarioAlterado = beneficiarios.FirstOrDefault(b => Regex.Replace(b.CPF, @"\D+", "") == Regex.Replace(model.CPF, @"\D+", ""));

            if (beneficiarioAlterado != null)
            {
                beneficiarios.Remove(beneficiarioAlterado);

                beneficiarios.Add(new Beneficiario()
                {
                    Nome = model.Nome,
                    CPF = Regex.Replace(model.CPF, @"\D", "")
                });

                _cacheService.Remover(lista);
                _cacheService.Salvar<List<Beneficiario>>(lista, beneficiarios, TimeSpan.FromMinutes(60));
            }
            else
            {
                beneficiarios.Add(new Beneficiario()
                {
                    Nome = model.Nome,
                    CPF = Regex.Replace(model.CPF, @"\D", "")
                });

                _cacheService.Remover(lista);
                _cacheService.Salvar<List<Beneficiario>>(lista, beneficiarios, TimeSpan.FromMinutes(60));
            }
        }

        public void Remover(string guid, Beneficiario model)
        {
            var lista = $"ListaBeneficiarios-{guid}";
            var beneficiarios = new List<Beneficiario>();

            beneficiarios = _cacheService.Obter<List<Beneficiario>>(lista);

            var beneficiarioAlterado = beneficiarios.FirstOrDefault(b => Regex.Replace(b.CPF, @"\D+", "") == Regex.Replace(model.CPF, @"\D+", ""));
            beneficiarios.Remove(beneficiarioAlterado);

            _cacheService.Remover(lista);
            _cacheService.Salvar<List<Beneficiario>>(lista, beneficiarios, TimeSpan.FromMinutes(60));
        }
    }
}