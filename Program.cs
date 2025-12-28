using Microsoft.OpenApi.Models;
using ziraat_webapi.Services;
using ziraat_webapi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ziraat_webapi.Interfaces;
using ziraat_webapi.Models;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<FinancialAssetsService>();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ziraat Web API", Version = "v1" });
});

builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddHttpClient<ICloudflareTurnstileService, CloudflareTurnstileService>();
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();


builder.Services.AddSingleton<OpenAIService>();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ziraat Web API v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

