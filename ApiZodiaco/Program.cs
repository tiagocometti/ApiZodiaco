using AstrologiaAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Porta dinâmica para o Render (usa $PORT ou 8080 localmente)
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "8080"));
});

builder.Services.AddControllers();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = JwtUtils.ObterParametrosValidacao();
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();  // Mantenha Swagger mesmo em produção para testes

// Remova UseHttpsRedirection() se não configurar HTTPS manualmente
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();