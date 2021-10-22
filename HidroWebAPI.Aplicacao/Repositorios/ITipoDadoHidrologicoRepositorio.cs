using System;
using System.Collections.Generic;
using System.Text;
using TipoDadoHidrologicoEntidade = HidroWebAPI.Dominio.Entidades.TipoDadoHidrologico;

namespace HidroWebAPI.Aplicacao.Repositorios
{
    public interface ITipoDadoHidrologicoRepositorio
    {
        IEnumerable<TipoDadoHidrologicoEntidade> ListarPorIdEmpreendedor(int IdEmpreendedor);
    }
}
