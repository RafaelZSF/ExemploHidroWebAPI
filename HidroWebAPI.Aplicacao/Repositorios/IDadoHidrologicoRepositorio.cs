using HidroWebAPI.Dominio.Entidades;

namespace HidroWebAPI.Aplicacao.Repositorios
{
    public interface IDadoHidrologicoRepositorio
    {
        int InserirDadoHidrologico(DadoHidrologico dadoHidrologicoEntidade);
    }
}
