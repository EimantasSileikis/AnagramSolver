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
        private readonly IConfiguration _config;

        public Settings(IConfiguration config)
        {
            _config = config;
        }

        public void SaveSettings()
        {
            var appConfig = new AppConfig();
            _config.Bind(appConfig);
            string json = JsonConvert.SerializeObject(appConfig);
            File.WriteAllText("appsettings.json", json);
        }
    }
}
