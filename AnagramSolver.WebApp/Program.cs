using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts.Interfaces;

Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IFileReader, FileReader>();
builder.Services.AddSingleton<IFileWriter, FileWriter>();
builder.Services.AddSingleton<IWordRepository, DbWordRepository>();
builder.Services.AddSingleton<IDbWordRepository, DbWordRepository>();
builder.Services.AddSingleton<IAnagramSolver, AnagramSolver.BusinessLogic.AnagramSolver>();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();