
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
            var builder = WebApplication.CreateBuilder(args);       // Criando um builder para a aplicação 

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(opt =>  // Adicionando o serviço de controllers e configurando o retorno de dados para Json
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());  // Permite que enums recebidos no body das requisições sejam convertidos para string
            });

            builder.Services.AddEndpointsApiExplorer();         // Adicionando o serviço de documentação da API
            builder.Services.AddSwaggerGen();                   // Adicionando o serviço de geração de documentação da API (Swagger)

            builder.Services.AddEntityFrameworkSqlServer().AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLDatabase")));

            builder.Services.AddAuthentication(options =>                   // Adicionando serviço de autenticação JWT via Bearer
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;        // Configurando Bearer Defaults para autenticação e challenge
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Events = new JwtBearerEvents                          // Configurando eventos do Bearer
                {

                    OnChallenge = async context =>                  // OnChallenge = Ocorre quando o token não é validado corretamente
                    {
                        context.HandleResponse();                                       // HandleResponse = Manipula a resposta do evento
                        context.Response.StatusCode = 401;                              // StatusCode = Status da resposta
                        await context.Response.WriteAsync("You are not authorized!");   // WriteAsync = Escreve uma string na resposta
                    }
                };

                o.TokenValidationParameters = new TokenValidationParameters             // Parâmetros de validação do Token 
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],                                                           // Issuer
                    ValidAudience = builder.Configuration["Jwt:Audience"],                                                       // Audience
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),       // Key
                    ValidateIssuer = false,                 // O issuer será validado?
                    ValidateAudience = false,               // O audience será validado?
                    ValidateLifetime = true,                // O Tempo de expiração será validado?
                    ValidateIssuerSigningKey = true         // A Key será validada?
                };
            });

            builder.Services.AddAuthorization();            // Adicionar serviço de autorização

            builder.Services.AddScoped<IUserRepository, UserRepository>();              // Adicionando dependência de injeção para o repositório de usuário
            builder.Services.AddScoped<IProductRepository, ProductRepository>();        // Adicionando dependência de injeção para o repositório de produto

            var app = builder.Build();                      // Criando a aplicação

            // Configurando o pipeline de requisições http
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();           // Usar o swagger em ambiente de desenvolvimento
                app.UseSwaggerUI();         // Usar o swagger UI em ambiente de desenvolvimento
            }

            app.UseHttpsRedirection();      // Redireciona requisições http para https

            app.UseAuthentication();        // Usa o serviço de autenticação
            app.UseAuthorization();         // Usa o serviço de autorização

            app.MapControllers();           // Mapeia os controllers

            app.Run();                      // Executa a aplicação
        }
    }
}