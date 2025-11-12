using Interfaces;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
    options.AddPolicy(name: "AllowBlazorClient",
        policy =>
        {
            policy.WithOrigins("http://72.61.44.192", "http://72.61.44.192:80", "http://petrakka.com/")
                .WithOrigins("https://72.61.44.192", "https://petrakka.com/")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // 1. Enable the Swagger middleware to generate the Swagger JSON document
    app.UseSwagger();
    
    // 2. Enable the Swagger UI middleware to serve the interactive UI
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
