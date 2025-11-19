using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

var bld = WebApplication.CreateBuilder(args);
bld.Services
   .AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(bld.Configuration.GetConnectionString("DefaultConnection")))
   .AddAuthenticationJwtBearer(s => s.SigningKey = bld.Configuration["Auth:JwtKey"])
   .AddAuthorization()
   .AddFastEndpoints(o => o.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All)
   .SwaggerDocument();

var app = bld.Build();
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