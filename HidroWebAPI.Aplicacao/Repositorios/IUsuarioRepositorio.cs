using System;
using System.Collections.Generic;
using System.Text;
using UsuarioEntidade = HidroWebAPI.Dominio.Entidades.Usuario;

namespace HidroWebAPI.Aplicacao.Repositorios
{
    public interface IUsuarioRepositorio
    {
        UsuarioEntidade VerificaLoginSenha(string Login, string Senha);
    }
}
