using Microsoft.AspNetCore.Authentication.Negotiate;
using System.Reflection.Metadata;
using Jbs.Yukari.Core.Data;
using Jbs.Yukari.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages();

// プロジェクトのサービスを追加
builder.Services.AddSingleton<IDatabase, Database>();
builder.Services.AddScoped<IQuery, Query>();
builder.Services.AddScoped<IRomanizer, Romanizer>();
builder.Services.AddScoped<IJsonSerializer, JsonSerializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
