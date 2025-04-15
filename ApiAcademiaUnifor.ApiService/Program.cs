using ApiAcademiaUnifor.ApiService.Service;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers(); 
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var supabaseUrl = "https://regjvzhfbouqrcgcghim.supabase.co";
var supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJlZ2p2emhmYm91cXJjZ2NnaGltIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDM4NTUzMzgsImV4cCI6MjA1OTQzMTMzOH0.Egzl6APxORjNv49UGEVNoHTpE4NF5MrqNbIXdtGBDX0";

var supabaseOptions = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};

var supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey, supabaseOptions);
await supabaseClient.InitializeAsync();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});



builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<GymEquipmentService>();
builder.Services.AddScoped<GymEquipmentCategoryService>();
builder.Services.AddScoped<ExerciseService>();
builder.Services.AddScoped<WorkoutService>();

builder.Services.AddSingleton(supabaseClient);


var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
}

app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
