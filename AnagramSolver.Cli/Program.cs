using AnagramSolver.BusinessLogic;
using AnagramSolver.Cli;
using AnagramSolver.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
var builder = new ConfigurationBuilder();
BuildConfig(builder);
builder.Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IFileReader, FileReader>();
        services.AddSingleton<IFileWriter, FileWriter>();
        services.AddSingleton<IWordRepository, DbWordRepository>();
        services.AddSingleton<IAnagramSolver, AnagramSolver.BusinessLogic.AnagramSolver>();
        services.AddSingleton<UI>();
    })
    .Build();

var anagramSolver = ActivatorUtilities.CreateInstance<AnagramSolver.BusinessLogic.AnagramSolver>(host.Services);
var appUI = ActivatorUtilities.CreateInstance<UI>(host.Services);

static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
}


