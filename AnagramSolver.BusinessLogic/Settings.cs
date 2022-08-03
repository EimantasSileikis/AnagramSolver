using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AnagramSolver.Contracts.Models;

namespace AnagramSolver.BusinessLogic
{
    public class Settings
    {
        public static Dictionary<string, int> settings = new Dictionary<string, int>();
        public static IConfiguration? configuration;

        public static void LoadSettings()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static void SaveSettings()
        {
            var appConfig = new AppConfig { };
            configuration.Bind(appConfig);
            string json = JsonConvert.SerializeObject(appConfig);
            File.WriteAllText("appsettings.json", json);
        }
    }
}
