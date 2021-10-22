using Microsoft.AspNetCore.Cors.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HidroWebAPI.Options.Cors
{
    public static class CustomCorsOptions
    {
        private static readonly CorsPolicy defaultPolicy = new CorsPolicyBuilder()
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .Build();

        public static Action<CorsOptions> SetupAction =>
            CustomCorsOptionsSetupAction;

        private static void CustomCorsOptionsSetupAction(CorsOptions corsOptions)
        {
            corsOptions.AddDefaultPolicy(defaultPolicy);
        }
    }
}
