using System;
using System.Collections.Generic;
using System.Text;
using LeituraEntidade = HidroWebAPI.Dominio.Entidades.Leitura;

namespace HidroWebAPI.Aplicacao.Repositorios
{
    public interface ILeituraRepositorio
    {
        int InserirLeitura(LeituraEntidade leituraEntidade);
    }
}
