
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using API_Rest_ASP_Core.Data;
using API_Rest_ASP_Core.Repositories.Interfaces;
using API_Rest_ASP_Core.Repositories;

namespace API_Rest_ASP_Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);       // Criando um builder para a aplica��o 

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(opt =>  // Adicionando o servi�o de controllers e configurando o retorno de dados para Json
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());  // Permite que enums recebidos no body das requisi��es sejam convertidos para string
            });

            builder.Services.AddEndpointsApiExplorer();         // Adicionando o servi�o de documenta��o da API
            builder.Services.AddSwaggerGen();                   // Adicionando o servi�o de gera��o de documenta��o da API (Swagger)

            builder.Services.AddEntityFrameworkSqlServer().AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLDatabase")));

            builder.Services.AddAuthentication(options =>                   // Adicionando servi�o de autentica��o JWT via Bearer
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;        // Configurando Bearer Defaults para autentica��o e challenge
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Events = new JwtBearerEvents                          // Configurando eventos do Bearer
                {

                    OnChallenge = async context =>                  // OnChallenge = Ocorre quando o token n�o � validado corretamente
                    {
                        context.HandleResponse();                                       // HandleResponse = Manipula a resposta do evento
                        context.Response.StatusCode = 401;                              // StatusCode = Status da resposta
                        await context.Response.WriteAsync("You are not authorized!");   // WriteAsync = Escreve uma string na resposta
                    }
                };

                o.TokenValidationParameters = new TokenValidationParameters             // Par�metros de valida��o do Token 
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],                                                           // Issuer
                    ValidAudience = builder.Configuration["Jwt:Audience"],                                                       // Audience
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),       // Key
                    ValidateIssuer = false,                 // O issuer ser� validado?
                    ValidateAudience = false,               // O audience ser� validado?
                    ValidateLifetime = true,                // O Tempo de expira��o ser� validado?
                    ValidateIssuerSigningKey = true         // A Key ser� validada?
                };
            });

            builder.Services.AddAuthorization();            // Adicionar servi�o de autoriza��o

            builder.Services.AddScoped<IUserRepository, UserRepository>();              // Adicionando depend�ncia de inje��o para o reposit�rio de usu�rio
            builder.Services.AddScoped<IProductRepository, ProductRepository>();        // Adicionando depend�ncia de inje��o para o reposit�rio de produto

            var app = builder.Build();                      // Criando a aplica��o

            // Configurando o pipeline de requisi��es http
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();           // Usar o swagger em ambiente de desenvolvimento
                app.UseSwaggerUI();         // Usar o swagger UI em ambiente de desenvolvimento
            }

            app.UseHttpsRedirection();      // Redireciona requisi��es http para https

            app.UseAuthentication();        // Usa o servi�o de autentica��o
            app.UseAuthorization();         // Usa o servi�o de autoriza��o

            app.MapControllers();           // Mapeia os controllers

            app.Run();                      // Executa a aplica��o
        }
    }
}