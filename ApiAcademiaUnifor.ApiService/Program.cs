using ApiAcademiaUnifor.ApiService.Service;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);


// Controllers + JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Swagger direto
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Supabase
var supabaseUrl = "https://regjvzhfbouqrcgcghim.supabase.co";
var supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJlZ2p2emhmYm91cXJjZ2NnaGltIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDM4NTUzMzgsImV4cCI6MjA1OTQzMTMzOH0.Egzl6APxORjNv49UGEVNoHTpE4NF5MrqNbIXdtGBDX0"; // (Truncado por segurança)

var supabaseOptions = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};

var supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey, supabaseOptions);
await supabaseClient.InitializeAsync();

builder.Services.AddSingleton(supabaseClient);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Seus serviços
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<GymEquipmentService>();
builder.Services.AddScoped<GymEquipmentCategoryService>();
builder.Services.AddScoped<ExerciseService>();
builder.Services.AddScoped<WorkoutService>();

var app = builder.Build();

// Configura tratamento de exceção com rota
app.UseExceptionHandler("/error");

app.UseCors("AllowAll");

// Endpoint para tratar exceções
app.Map("/error", (HttpContext httpContext) =>
{
    var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
    var exception = exceptionHandlerFeature?.Error;

    return Results.Problem(title: exception?.Message);
});

if (app.Environment.IsDevelopment())
{
    // Ativa o Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
