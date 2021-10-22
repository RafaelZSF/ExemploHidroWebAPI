using HidroWebAPI.Aplicacao.Repositorios;
using HidroWebAPI.Infraestrutura.Contexto;
using HidroWebAPI.Util.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using UsuarioEntidade = HidroWebAPI.Dominio.Entidades.Usuario;

namespace HidroWebAPI.Infraestrutura.BancoDados.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly EntityRepositorioBase<UsuarioEntidade> usuarioRepositorio;

        public UsuarioRepositorio(HidroContexto contextdb)
        {
            usuarioRepositorio = new EntityRepositorioBase<UsuarioEntidade>(contextdb);
        }

        public UsuarioEntidade VerificaLoginSenha(string Login, string Senha)
        {
            HidroContexto usuarioContexto = usuarioRepositorio.context;

            IEnumerable<UsuarioEntidade> enumUsuarioEntidade =
                            from usuario in usuarioContexto.Usuario
                             where usuario.Login == Login 
                             && usuario.Senha == Senha
                             select usuario;

            UsuarioEntidade usuarioEntidade = enumUsuarioEntidade.FirstOrDefault();

            if (usuarioEntidade == null)
                throw new AuthenticationException("As credenciais do usuário não são válidas");

            return usuarioEntidade;
        }

    }

}


