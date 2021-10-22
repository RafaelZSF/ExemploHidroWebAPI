using System;
using System.Collections.Generic;
using System.Text;
using ViewGestaoBarragensEntidade = HidroWebAPI.Dominio.Entidades.VS_GESTAO_BARRAGENS;

namespace HidroWebAPI.Aplicacao.Repositorios
{
    public interface IViewGestaoBarragensRepositorio
    {
        IEnumerable<ViewGestaoBarragensEntidade> ListarPorData(DateTime DataInicio, DateTime DataFim);
    }
}
