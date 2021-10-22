using HidroWebAPI.Aplicacao.Repositorios;
using HidroWebAPI.Infraestrutura.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TipoDadoHidrologicoEntidade = HidroWebAPI.Dominio.Entidades.TipoDadoHidrologico;

namespace HidroWebAPI.Infraestrutura.BancoDados.Repositorios
{
    public class TipoDadoHidrologicoRepositorio : ITipoDadoHidrologicoRepositorio
    {
        private readonly EntityRepositorioBase<TipoDadoHidrologicoEntidade> tipoDadoHidrologicoRepositorio;

        public TipoDadoHidrologicoRepositorio(HidroContexto contextdb)
        {
            tipoDadoHidrologicoRepositorio = new EntityRepositorioBase<TipoDadoHidrologicoEntidade>(contextdb);
        }

        public IEnumerable<TipoDadoHidrologicoEntidade> ListarPorIdEmpreendedor(int IdEmpreendedor)
        {
            HidroContexto tipoDadoHidrologicoContexto = tipoDadoHidrologicoRepositorio.context;

            IEnumerable<TipoDadoHidrologicoEntidade> enumTipoDadoHidrologico =
                            from tipoDadoHidrologico in tipoDadoHidrologicoContexto.TipoDadoHidrologico
                             where tipoDadoHidrologico.IdEmpreendedor == IdEmpreendedor
                             select tipoDadoHidrologico;

            return enumTipoDadoHidrologico;
        }

    }
}
