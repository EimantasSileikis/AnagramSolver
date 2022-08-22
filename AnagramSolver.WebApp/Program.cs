using AnagramSolver.BusinessLogic.Core;
using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.EF.CodeFirst;
using AnagramSolver.EF.CodeFirst.Data;
using Microsoft.EntityFrameworkCore;

Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CodeFirstContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("CodeFirstDb")));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAnagramSolver, DbAnagramSolver>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "anagrams",
    pattern: "Anagrams/{word?}",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();