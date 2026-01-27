using DTOs;
using Interfaces;
using MercadoPago.Config;
using Services;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;

var builder = WebApplication.CreateBuilder(args);
var accessToken = builder.Configuration["MercadoPago:AccessToken"]
                  ?? Environment.GetEnvironmentVariable("MERCADOPAGO_ACCESS_TOKEN")
                  ?? throw new InvalidOperationException("MercadoPago AccessToken não configurado.");

// Configuração global do SDK
BsonSerializer.RegisterSerializer(new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));
MercadoPagoConfig.AccessToken = accessToken;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pet Monitoring API", // Give your API a descriptive title
        Version = "v1",
        Description = "API for managing animal health and geolocation records."
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantService, TenantService>();

// Dependency Injection Setup
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
builder.Services.AddScoped<IMedicacaoService, MedicacaoService>();
builder.Services.AddScoped<IDiagnosticoService, DiagnosticoService>();
builder.Services.AddScoped<ICirurgiaService, CirurgiaService>();
builder.Services.AddScoped<IRitmoCircadianoService, RitmoCircadianoService>();
builder.Services.AddScoped<IHistoryFrequenciaCardiaca, HistoryFrequenciaCardiacaService>();
builder.Services.AddScoped<IFrequenciaCardiaca, FrequenciaCardiacaService>();
builder.Services.AddScoped<IAtendenteService, AtendenteService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<AgendamentoService>();
builder.Services.AddScoped<AtendimentoService>();
builder.Services.AddScoped<DisponibilidadeService>();
builder.Services.AddHttpClient<IPaymentGateway, PaymentGateway>();
builder.Services.AddScoped(typeof(IRepositorio<Order>), typeof(Repositorio));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pet Monitoring API v1");
    });
}

// ORDEM CRÍTICA: UseCors ANTES de MapControllers
app.UseHttpsRedirection();

app.UseCors("AllowAll"); // ESSA LINHA ESTAVA FALTANDO!

app.MapControllers();

app.Run();