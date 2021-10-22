using HidroWebAPI.Infraestrutura.Contexto;
using System;
using System.Collections.Generic;
using System.Text;
using LeituraEntidade = HidroWebAPI.Dominio.Entidades.Leitura;
using System.Linq;
using HidroWebAPI.Aplicacao.Repositorios;

namespace HidroWebAPI.Infraestrutura.BancoDados.Repositorios
{
    public class LeituraRepositorio : ILeituraRepositorio
    {
        private readonly EntityRepositorioBase<LeituraEntidade> leituraRepositorio;

        public LeituraRepositorio(HidroContexto contextdb)
        {
            leituraRepositorio = new EntityRepositorioBase<LeituraEntidade>(contextdb);
        }

        public int InserirLeitura(LeituraEntidade leituraEntidade)
        {
            leituraRepositorio.Insert(leituraEntidade);
            leituraRepositorio.Commit();
            return leituraEntidade.IdLeitura;
        }
    }
}

