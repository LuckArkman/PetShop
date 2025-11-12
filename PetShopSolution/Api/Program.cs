using Interfaces;
using Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddScoped<ICaixaService, CaixaService>();
builder.Services.AddScoped<IAtendenteService, AtendenteService>();
builder.Services.AddScoped<AgendamentoService>();
builder.Services.AddScoped<AtendimentoService>();
builder.Services.AddScoped<DisponibilidadeService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        // Lista completa de origens permitidas
        var origins = new[]
        {
            "http://localhost",
            "http://127.0.0.1",
            "http://72.61.44.192",
            "http://72.61.44.192",
            "http://petrakka.com",
            "https://72.61.44.192",
            "https://petrakka.com"
        };

        policy.WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Necessário para cookies, JWT em cookies, etc.
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

app.UseCors("AllowBlazorClient"); // ESSA LINHA ESTAVA FALTANDO!

app.MapControllers();

app.Run();