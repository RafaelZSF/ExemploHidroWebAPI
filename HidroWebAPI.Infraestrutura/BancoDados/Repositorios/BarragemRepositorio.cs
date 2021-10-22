using HidroWebAPI.Aplicacao.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;
using BarragemEntidade = HidroWebAPI.Dominio.Entidades.Barragem;
using System.Linq;
using HidroWebAPI.Infraestrutura.Contexto;

namespace HidroWebAPI.Infraestrutura.BancoDados.Repositorios
{
    public class BarragemRepositorio : IBarragemRepositorio
    {
        private readonly EntityRepositorioBase<BarragemEntidade> barragemRepositorio;

        public BarragemRepositorio(HidroContexto contextdb)
        {
            barragemRepositorio = new EntityRepositorioBase<BarragemEntidade>(contextdb);
        }

        public IEnumerable<BarragemEntidade> ListarPorIdEmpreendedor(int IdEmpreendedor)
        {
            HidroContexto barragemContexto = barragemRepositorio.context;

            IEnumerable<BarragemEntidade> enumBarragem =
                            (from barragem in barragemContexto.Barragem
                             where barragem.IdEmpreendedor == IdEmpreendedor 
                             && barragem.DataExclusao == null
                             select barragem);

            return enumBarragem;
        }
    }
}
