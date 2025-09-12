// To run the server, configure the Strava client secret first.
// You can find it at https://www.strava.com/settings/api

// dotnet user-secrets init
// dotnet user-secrets set "Strava:ClientSecret" "..."

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions
    {
      Args = args,
      WebRootPath = Path.Combine(AppContext.BaseDirectory, "wwwroot")
    });

var stravaSecret = builder.Configuration["Strava:ClientSecret"];
if (string.IsNullOrWhiteSpace(stravaSecret))
{
  LoggerFactory
      .Create(logging => logging.AddConsole())
      .CreateLogger("StartupCheck")
      .LogWarning("Strava ClientSecret is missing. API calls will fail.");
}

builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache(); // For sessions
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache(); // For Strava API responses
builder.Services.AddOpenApi();  // See more at https://aka.ms/aspnet/openapi
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromHours(1); // adjust as needed
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});

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

//app.UseHttpsRedirection();
app.UseDefaultFiles(); // serve index.html by default
app.UseStaticFiles();  // serve React build
app.UseSession();
app.MapControllers();
app.MapFallbackToFile("index.html"); // serve React for other routes

app.Run();
