using File.Infrastructure.Repositories;
using Files.Domain.Models;
using Infrastructer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepository<FileUploade>, FileRepository>();

builder.Services.AddDbContext<FileContext>(ServiceLifetime.Scoped);
/* ���� ���� �������� ����� ��� �������� �� Cookie Authentication.
.AddCookie(): ���� ������ ������ ������ ������ҡ ��� ���� ����� ������ ���� ������ ��������.*/

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    option =>
    {
        option.LoginPath = "/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        option.SlidingExpiration = true;
    });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles(); // ���� ������ ������� ������� ��� ����� ������ �������.

app.UseRouting(); // ���� ������ ���� ������� ���� ���� ������ �� ��� ���� ������� ��� �� ����

app.UseAuthentication(); // ����� ����� ��������

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ����� ���� ����� ������ ����� ��������
app.MapControllerRoute(
    name: "Login",
    pattern: "Login",
    defaults: new { controller = "Login", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FileUploades}/{action=Index}/{id?}");
app.Run();
