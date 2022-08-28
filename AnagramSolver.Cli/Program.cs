using AnagramSolver.BusinessLogic.Data;
using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Cli;
using AnagramSolver.Contracts.Interfaces.Core;
using AnagramSolver.Contracts.Interfaces.Files;
using AnagramSolver.Contracts.Interfaces.Repositories;
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
        services.AddSingleton<IFileManager, FileManager>();
        services.AddSingleton<IWordRepository, WordRepository>();
        services.AddSingleton<IAnagramSolver, AnagramSolver.BusinessLogic.Services.AnagramSolver>();
        services.AddSingleton<UI>();
    })
    .Build();

var anagramSolver = ActivatorUtilities.CreateInstance<AnagramSolver.BusinessLogic.Services.AnagramSolver>(host.Services);
var appUI = ActivatorUtilities.CreateInstance<UI>(host.Services);

static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
}


