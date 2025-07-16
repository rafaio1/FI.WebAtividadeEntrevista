using System;
using System.Runtime.Caching;

namespace WebAtividadeEntrevista.Service
{
    public class WebCacheService
    {
        private readonly ObjectCache _cache = MemoryCache.Default;

        public T Obter<T>(string chave) where T : class
        {
            return _cache.Get(chave) as T;
        }

        public void Salvar<T>(string chave, T valor, TimeSpan expiracao) where T : class
        {
            _cache.Set(chave, valor, DateTimeOffset.Now.Add(expiracao));
        }

        public void Remover(string chave)
        {
            _cache.Remove(chave);
        }
    }
}