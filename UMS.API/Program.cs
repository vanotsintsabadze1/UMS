using UMS.API.Infrastructure.Extensions;
using UMS.Application.Extensions;
using UMS.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureRouting();
builder.Services.ConfigureControllers();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureValidation();

builder.Logging.ConfigureLogging();

builder.Services.AddInfrastructureServices()
    .AddApplicationServices();

var app = builder.Build();

app.UseConfiguredSwagger();
app.UseVersioning();
app.UseHttpsRedirection();
app.UseRouting();
app.UseGlobalExceptionHandling();
app.MapControllers();

app.Run();