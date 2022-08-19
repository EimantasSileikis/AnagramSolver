using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.EF.DatabaseFirst.Data;
using Microsoft.EntityFrameworkCore;

Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AnagramsContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("WordsDatabase")));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFileReader, FileReader>();
builder.Services.AddScoped<IFileWriter, FileWriter>();
builder.Services.AddScoped<IWordRepository, WordRepository>();
builder.Services.AddScoped<IDbWordRepository, AnagramSolver.EF.DatabaseFirst.WordRepository> ();
builder.Services.AddScoped<IAnagramSolver, AnagramSolver.BusinessLogic.AnagramSolver>();


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