using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OIDC.GrpcClients;
using OIDC.Jwt;
using OIDC.Services;
using SessionManager;

var builder = WebApplication.CreateBuilder(args);

// Create RSA key once and keep it alive
var rsa = RSA.Create(2048);
var rsaSecurityKey = new RsaSecurityKey(rsa)
{
    KeyId = builder.Configuration["Oidc:KeyId"] ?? Guid.NewGuid().ToString("N")
};
builder.Services.AddSingleton(new RsaKeyService(rsaSecurityKey));
builder.Services.AddSingleton<IdTokenGenerator>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddGrpcClients();
builder.Services.AddServices();
builder.Services.AddRedisSessionManagement(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
