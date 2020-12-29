using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Common.Domain.Entities;
using Newtonsoft.Json;

namespace Common.Domain.Configurations
{
    public class ApplicationConfiguration
    {
        public ApplicationConfiguration()
        {
            Users = new List<User>();
        }

        [JsonIgnore] public string ConfigurationDirectory => @"..\..\..\..\..\Resources\Tests.Data\configurations";

        [JsonIgnore] public string ConfigurationFilePath => Path.Combine(ConfigurationDirectory, "configuration.json");

        [JsonIgnore] public bool ConfigurationFileExists => File.Exists(ConfigurationFilePath);

        [JsonIgnore] public string UserImagesDirectory => Path.Combine(ConfigurationDirectory, "images");

        public List<User> Users { get; set; }

        public void Load()
        {
            var applicationConfiguration = JsonConvert.DeserializeObject<ApplicationConfiguration>(File.ReadAllText(ConfigurationFilePath));

            Users = applicationConfiguration.Users;
        }

        public void Save()
        {
            try
            {
                //Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), ConfigurationDirectory));

                if (!Directory.Exists(ConfigurationDirectory))
                {
                    Directory.CreateDirectory(ConfigurationDirectory);
                }

                if (!Directory.Exists(UserImagesDirectory))
                {
                    Directory.CreateDirectory(UserImagesDirectory);
                }

                var jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(ConfigurationFilePath, jsonString);
            }
            catch (Exception)
            {
                throw new Exception($"Configuration Error: Could not create configuration file. Manually create at \n {ConfigurationFilePath}");
            }
        }
    }
}