var builder = WebApplication.CreateBuilder(args);

// Configurar puerto ANTES de build
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Solo HTTPS redirect en desarrollo
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseCors();
app.MapControllers();

app.MapGet("/", () => "Chatbot API is running! Available endpoints: /weatherforecast, /api/chat");

app.MapPost("/api/chat", async (ChatRequest request) =>
{
    var response = request.Message.ToLower() switch
    {
        "hola" => "¡Hola! ¿Cómo puedo ayudarte?",
        "date" => $"La fecha actual es: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
        "clima" => "Para el clima, usa /weatherforecast",
        _ => $"Recibí tu mensaje: '{request.Message}'. ¿En qué más puedo ayudarte?"
    };
return new ChatResponse 
    { 
        Reply = response,
        Timestamp = DateTime.Now
    };
});



var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
public record ChatRequest(string Message);
public record ChatResponse
{
    public string Reply { get; set; } = "";
    public DateTime Timestamp { get; set; }
}