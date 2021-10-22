using System;
using System.Collections.Generic;
using System.Text;
using InstrumentoEntidade = HidroWebAPI.Dominio.Entidades.Instrumento;

namespace HidroWebAPI.Aplicacao.Repositorios
{
    public interface IInstrumentoRepositorio
    {
        IEnumerable<InstrumentoEntidade> ListarPorArrIdInstrumento(int[] ArrIdInstrumento);
    }
}
