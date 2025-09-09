// To run the server, configure the Strava client secret first.
// You can find it at https://www.strava.com/settings/api

// dotnet user-secrets init
// dotnet user-secrets set "Strava:ClientSecret" "..."

using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(
  new WebApplicationOptions
  {
    Args = args,
    WebRootPath = Path.Combine("..", "client", "dist")
  });
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddOpenApi();  // See more at https://aka.ms/aspnet/openapi

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/openapi/v1.json", "API v1");
  });
}

app.UseHttpsRedirection();
app.UseDefaultFiles(); // serve index.html by default
app.UseStaticFiles();  // serve React build
app.MapControllers();

app.Run();
