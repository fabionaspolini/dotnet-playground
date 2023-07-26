using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);

// Adicionar scope no log com o traceId gerado com new Activity("...")
builder.Logging.Configure(opts => opts.ActivityTrackingOptions = ActivityTrackingOptions.SpanId |
    ActivityTrackingOptions.TraceId |
    ActivityTrackingOptions.ParentId |
    ActivityTrackingOptions.Tags |
    ActivityTrackingOptions.Baggage);

/*builder.Logging.AddJsonConsole(x =>
{
    x.IncludeScopes = true;
    x.JsonWriterOptions = new()
    {
        Indented = true, // Indentação causa muita queda de performance
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
});*/
/*builder.Logging.AddMyJsonFormatterConsole(x =>
{
    x.IncludeScopes = true;
    x.TimestampFormat = "O";
    x.JsonWriterOptions = new()
    {
        Indented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
});*/
builder.Logging.AddMyJsonFormatterConsole();

// Add services to the container.

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

app.UseAuthorization();

app.MapControllers();

app.Run();
