using UMS.API.Infrastructure.Extensions;
using UMS.Application.Extensions;
using UMS.Infrastructure.Extensions;
using UMS.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureRouting();
builder.Services.ConfigureControllers();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureValidation();
builder.Services.AddLocalization();

builder.Logging.ConfigureLogging();

builder.Services.AddInfrastructureServices()
    .AddApplicationServices();

var app = builder.Build();

app.UseConfiguredLocalization();
app.UseConfiguredSwagger();
app.UseVersioning();
app.UseHttpsRedirection();
app.UseRouting();
app.UseGlobalExceptionHandling();
app.MapControllers();

if (builder.Environment.IsDevelopment())
{
    await DbMigrator.Migrate(app.Services);
}

app.Run();