using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

var bld = WebApplication.CreateBuilder(args);

// Configure URLs to listen on all network interfaces
bld.WebHost.UseUrls("http://0.0.0.0:5000");

// Add CORS policy to allow all origins
bld.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

bld.Services
   .AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(bld.Configuration.GetConnectionString("DefaultConnection")))
   .AddAuthenticationJwtBearer(s => s.SigningKey = bld.Configuration["Auth:JwtKey"])
   .AddAuthorization()
   .AddFastEndpoints(o => o.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All)
   .SwaggerDocument();

var app = bld.Build();

// Use CORS policy
app.UseCors("AllowAll");

app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints(
       c =>
       {
           c.Binding.ReflectionCache.AddFromPulsApi();
           c.Errors.UseProblemDetails();
       })
   .UseSwaggerGen();
app.Run();

public partial class Program;