using UMS.API.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureValidation();

var app = builder.Build();

app.UseConfiguredSwagger();
app.UseHttpsRedirection();
app.UseRouting();

app.Run();