using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PetShop.Application.Data;
using PetShop.Application.Interfaces;
using PetShop.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoDbService>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoDbService, MongoDbService>();
// Register Application Services
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IAnimalGeolocationHistoryService, AnimalGeolocationHistoryService>();
builder.Services.AddScoped<IGeolocationRecordService, GeolocationRecordService>();
builder.Services.AddScoped<IHistoricoClinicoService, HistoricoClinicoService>();
builder.Services.AddScoped<IMedicoVeterinarioService, MedicoVeterinarioService>();
builder.Services.AddScoped<IQrCodeRegistroService, QrCodeRegistroService>();
builder.Services.AddScoped<IResponsavelService, ResponsavelService>();
builder.Services.AddScoped<IRelatorioClinicoService, RelatorioClinicoService>();
builder.Services.AddScoped<IHistoryVacinacaoService, HistoryVacinacaoService>();
builder.Services.AddScoped<IVacinacaoService, VacinacaoService>();
builder.Services.AddScoped<IRitmoCircadianoService, RitmoCircadianoService>();
builder.Services.AddScoped<IHistoryFrequenciaCardiaca, HistoryFrequenciaCardiacaService>();
builder.Services.AddScoped<IFrequenciaCardiaca, FrequenciaCardiacaService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");
app.MapControllers();
app.Run();