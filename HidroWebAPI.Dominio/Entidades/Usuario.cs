using System;
using System.Collections.Generic;
using System.Text;

namespace HidroWebAPI.Dominio.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public int IdEmpreendedor { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
    }
}
