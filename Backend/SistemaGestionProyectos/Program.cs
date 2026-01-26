using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaGestionProyectos.Dominio.Modelo;
using SistemaGestionProyectos.Dominio.Repositories;
using SistemaGestionProyectos.Infraestructura.Datos;
using SistemaGestionProyectos.Infraestructura.Repositories;
using SistemaGestionProyectos.Services.Login;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProyectosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IGenericoRepository<Proyecto>, ProyectoRepository>();
builder.Services.AddTransient<IGenericoRepository<Equipo>, EquipoRepository>();
builder.Services.AddTransient<IGenericoRepository<Usuario>, UsuarioRepository>();
builder.Services.AddTransient<IGenericoRepository<ProyectoEquipo>, ProyectoEquipoRepository>();
builder.Services.AddTransient<IProyectoEquipoRepository, ProyectoEquipoRepository>();
//builder.Services.AddTransient<IProyectoRepository, ProyectoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add services to the container.

// Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Permitir solo tu frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddHttpClient("SASI", client =>
{
    client.BaseAddress = new Uri("https://localhost:44337/");
});

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar la política de CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
