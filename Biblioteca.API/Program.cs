using Microsoft.EntityFrameworkCore;
using Biblioteca.Data.Context;
using Biblioteca.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Conex�o com o banco
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));

// Inje��o de depend�ncia
builder.Services.AddScoped<LivroRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();