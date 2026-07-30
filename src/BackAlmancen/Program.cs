using BackAlmancen;
using BackAlmancen.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ContextDB>();
    context.Database.EnsureCreated();
}

app.Run();
