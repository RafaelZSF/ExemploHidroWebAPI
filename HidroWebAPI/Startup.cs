using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HidroWebAPI.Options.Cors;
using HidroWebAPI.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using HidroWebAPI.Aplicacao;
using Microsoft.AspNetCore.Http;
using HidroWebAPI.Filters;
using HidroWebAPI.Models.Responses.Http;
using HidroWebAPI.Utils;
using HidroWebAPI.Middlewares;

namespace HidroWebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddCors(CustomCorsOptions.SetupAction);

            //services.AddControllers();

            //ToDo: Escrever Options Customizadas, evitando 'poluir' o fluxo do Startup.cs
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });

            ////ToDo: Escrever Options Customizadas, evitando 'poluir' o fluxo do Startup.cs
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
            {
                string sec = Configuration.GetSection("JwtBearerOptions:IssuerSigningKey").Value;
                string audience = Configuration.GetSection("JwtBearerOptions:ValidAudience").Value;
                string issuer = Configuration.GetSection("JwtBearerOptions:ValidIssuer").Value;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Base64UrlTextEncoder.Decode(sec))
                };
                jwtBearerOptions.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        context.HttpContext.User = context.Principal;
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        // ToDo: Log contexto avisando tentativa falha de autenticacao
                        Console.WriteLine("Autenticacao falhou!");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        // ToDo: Log contexto avisando usuario com token Invalido
                        Console.WriteLine("Usuario com token invalido!");
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        // ToDo: Log contexto avisando usuario nao autorizado
                        Console.WriteLine("Usuario nao autorizado!");
                        return Task.CompletedTask;
                    },
                };
            });

            ////ToDo: Escrever Options Customizadas, evitando 'poluir' o fluxo do Startup.cs
            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireClaim(ClaimTypes.NameIdentifier)
                    .Build();
            });

            services.AddControllers(mvcOptions =>
            {
                mvcOptions.Filters.Add(new ProducesResponseTypeAttribute(typeof(BadRequestResponse), StatusCodes.Status400BadRequest));
                mvcOptions.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), StatusCodes.Status401Unauthorized));
                mvcOptions.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), StatusCodes.Status403Forbidden));
                mvcOptions.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), StatusCodes.Status404NotFound));
                mvcOptions.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorResponse), StatusCodes.Status500InternalServerError));
                mvcOptions.Filters.Add<ValidationFilter>();
                mvcOptions.Filters.Add<ExceptionFilter>();
                //mvcOptions.ModelBinderProviders.Insert(0, new CultureInvariantDoubleBinderProvider());
            })
            .AddJsonOptions(jsonOptions => jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null);


            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Hidro API", Version = "v1" });
                string xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                swaggerGenOptions.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}_{apiDesc.RelativePath}");
                swaggerGenOptions.IncludeXmlComments(xmlFilePath);
            //Todo: Ver erro
                //swaggerGenOptions.AddFluentValidationRules();

                swaggerGenOptions.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Description = $"JWT Authorization header using the {JwtBearerDefaults.AuthenticationScheme} scheme.",
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });
                swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
            });

            services.AddDbContext<HidroContexto>(opt => opt.UseSqlServer(Configuration.GetConnectionString("SYSDAM_CONEXAO")));

            services.AddMvc().AddControllersAsServices();

            services.AddApplication();
            DependencyInjection.RegisterServices(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                ExceptionHandler = ExceptionHandlerEvents.OnExceptionAsync
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            //ToDo: Escrever Options Customizadas, evitando 'poluir' o fluxo do Startup.cs
            app.UseRequestLocalization(requestLocalizationOptions => {
                string[] supportedCultures = new[] { "pt-BR", "en-US", "es-ES" };
                requestLocalizationOptions
                    .SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();

                //endpoints.MapControllerRoute(
                //name: "DefaultApi",
                //pattern: "api/{controller}/{id}");

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();

            //ToDo: Escrever Options Customizadas, evitando 'poluir' o fluxo do Startup.cs
            app.UseSwaggerUI(swaggerUIOptions =>
            {
                swaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Hidro API V1");
                swaggerUIOptions.RoutePrefix = string.Empty;
                swaggerUIOptions.DisplayRequestDuration();
                swaggerUIOptions.EnableFilter();
                swaggerUIOptions.EnableDeepLinking();
                swaggerUIOptions.DefaultModelsExpandDepth(-1);
            });
        }
    }
}
