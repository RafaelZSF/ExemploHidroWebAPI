using HidroWebAPI.Aplicacao.Repositorios;
using HidroWebAPI.Infraestrutura.Contexto;
using System;
using System.Collections.Generic;
using System.Text;
using DadoHidrologicoEntidade = HidroWebAPI.Dominio.Entidades.DadoHidrologico;

namespace HidroWebAPI.Infraestrutura.BancoDados.Repositorios
{
    public class DadoHidrologicoRepositorio : IDadoHidrologicoRepositorio
    {
        private readonly EntityRepositorioBase<DadoHidrologicoEntidade> dadoHidrologicoRepositorio;

        public DadoHidrologicoRepositorio(HidroContexto contextdb)
        {
            dadoHidrologicoRepositorio = new EntityRepositorioBase<DadoHidrologicoEntidade>(contextdb);
        }

        public int InserirDadoHidrologico(DadoHidrologicoEntidade dadoHidrologicoEntidade)
        {
            dadoHidrologicoRepositorio.Insert(dadoHidrologicoEntidade);
            dadoHidrologicoRepositorio.Commit();
            return dadoHidrologicoEntidade.IdDadoHidrologico;
        } 
    }
}
