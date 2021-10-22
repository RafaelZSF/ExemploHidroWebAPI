using HidroWebAPI.Aplicacao.Repositorios;
using HidroWebAPI.Infraestrutura.Contexto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ViewGestaoBarragensEntidade = HidroWebAPI.Dominio.Entidades.VS_GESTAO_BARRAGENS;

namespace HidroWebAPI.Infraestrutura.BancoDados.Repositorios
{
    public class ViewGestaoBarragensRepositorio : IViewGestaoBarragensRepositorio
    {
        private readonly EntityRepositorioBase<ViewGestaoBarragensEntidade> viewGestaoBarragensRepositorio;

        public ViewGestaoBarragensRepositorio(HidroContexto contextdb)
        {
            viewGestaoBarragensRepositorio = new EntityRepositorioBase<ViewGestaoBarragensEntidade>(contextdb);
        }

        public IEnumerable<ViewGestaoBarragensEntidade> ListarPorData(DateTime DataInicio,DateTime DataFim)
        {
            HidroContexto viewGestaoBarragensContexto = viewGestaoBarragensRepositorio.context;

            IEnumerable<ViewGestaoBarragensEntidade> enumDadosHidro =
                            from viewGestaoBarragens in viewGestaoBarragensContexto.VS_GESTAO_BARRAGENS
                            where viewGestaoBarragens.DT_REGISTRO >= DataInicio
                            && viewGestaoBarragens.DT_REGISTRO <= DataFim
                            select viewGestaoBarragens;

            return enumDadosHidro;
        }
    }

}
