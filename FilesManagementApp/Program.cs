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
/* Ì÷Ì› Œœ„… «·„’«œﬁ… ÊÌÕœœ ‰Ê⁄ «·„’«œﬁ… ﬂ‹ Cookie Authentication.
.AddCookie(): ÌﬁÊ„ » ﬂÊÌ‰ „Õœœ«  „’«œﬁ… «·ﬂÊﬂÌ“° „À· „”«—  ”ÃÌ· «·œŒÊ· ÊÊﬁ  «‰ Â«¡ «·’·«ÕÌ….*/

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
app.UseStaticFiles(); // Ì”„Õ » ﬁœÌ„ «·„·›«  «·À«» … „À· «·’Ê— Ê„·›«  «·√‰„«ÿ.

app.UseRouting(); // ÌﬁÊ„ » ﬂÊÌ‰ ‰Ÿ«„ «· ÊÃÌÂ «·–Ì ÌﬁÊ„ » ÕœÌœ „« ≈–« ﬂ«‰  «·ÿ·»«  ÌÃ» √‰  ÊÃÂ

app.UseAuthentication(); //  ›⁄Ì· «·œ⁄„ ··„’«œﬁ…

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//  ÕœÌœ ’›Õ…  ”ÃÌ· «·œŒÊ· ﬂ’›Õ… «» œ«∆Ì…
app.MapControllerRoute(
    name: "Login",
    pattern: "Login",
    defaults: new { controller = "Login", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FileUploades}/{action=Index}/{id?}");
app.Run();
