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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // Update UseSwaggerUI to explicitly reference the "v1" document for clarity
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pet Monitoring API v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();