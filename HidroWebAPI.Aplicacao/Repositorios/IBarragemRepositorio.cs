using System;
using System.Collections.Generic;
using System.Text;
using BarragemEntidade = HidroWebAPI.Dominio.Entidades.Barragem;

namespace HidroWebAPI.Aplicacao.Repositorios
{
    public interface IBarragemRepositorio
    {
        IEnumerable<BarragemEntidade> ListarPorIdEmpreendedor(int IdEmpreendedor);
    }
}
