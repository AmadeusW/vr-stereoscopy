using System;
using Microsoft.Extensions.Configuration;

namespace StereoscopyVR.CoreData
{
    public static class Configuration
    {
        static public IConfigurationRoot Instance { get; set; }

        public static void Initialize()
        {
            var configPath = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\OneDrive\current\vrcv.json");
            var builder = new ConfigurationBuilder()
                .AddJsonFile(configPath);
            Instance = builder.Build();
        }
    }
}
