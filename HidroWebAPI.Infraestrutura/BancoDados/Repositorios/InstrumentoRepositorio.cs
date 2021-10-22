using HidroWebAPI.Aplicacao.Repositorios;
using HidroWebAPI.Infraestrutura.Contexto;
using System;
using System.Collections.Generic;
using System.Text;
using InstrumentoEntidade = HidroWebAPI.Dominio.Entidades.Instrumento;
using System.Linq;

namespace HidroWebAPI.Infraestrutura.BancoDados.Repositorios
{
    public class InstrumentoRepositorio : IInstrumentoRepositorio
    {
        private readonly EntityRepositorioBase<InstrumentoEntidade> instrumentoRepositorio;

        public InstrumentoRepositorio(HidroContexto contextdb)
        {
            instrumentoRepositorio = new EntityRepositorioBase<InstrumentoEntidade>(contextdb);
        }

        public IEnumerable<InstrumentoEntidade> ListarPorArrIdInstrumento(int[] ArrIdInstrumento)
        {
            HidroContexto instrumentoContexto = instrumentoRepositorio.context;

            IEnumerable<InstrumentoEntidade> enumInstrumento =
                            (from instrumento in instrumentoContexto.Instrumento
                             where ArrIdInstrumento.Contains(instrumento.IdInstrumento)
                             select instrumento);

            return enumInstrumento;
        }

    }
}
