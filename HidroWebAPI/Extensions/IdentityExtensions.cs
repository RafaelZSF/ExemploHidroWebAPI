using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace HidroWebAPI.Extensions
{
    public static class IdentityExtensions
    {
        public static int ObterIdUsuario(this IIdentity identity)
        {
            IEnumerable<Claim> claims = ((ClaimsIdentity)identity).Claims;

            // Como a aplicacao possui como requisito de autorizacao uma claim do tipo NameIdentifier,
            // nao precisamos verificar se o mesmo existe.

            return Convert.ToInt32(claims.First(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        }
    }
}
