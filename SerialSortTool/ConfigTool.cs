using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SerialSortTool
{
    public class ConfigTool
    {
        private IConfiguration _configuration;

        public ConfigTool()
        {
            //在当前目录或者根目录中寻找appsettings.json文件
            var fileName = "appsettings.json";

            var directory = AppContext.BaseDirectory;
            directory = directory.Replace("\\", "/");

            var filePath = $"{directory}/{fileName}";
            if (!File.Exists(filePath))
            {
                var length = directory.IndexOf("/bin");
                filePath = $"{directory.Substring(0, length)}/{fileName}";
            }

            var builder = new ConfigurationBuilder()
                .AddJsonFile(filePath, false, true);

            _configuration = builder.Build();
        }

        public string this[string key]
        {
            get
            {
                return GetSectionValue(key);
            }
        }

        public string GetSectionValue(string key)
        {
            return _configuration.GetSection(key).Value;
        }

        public T GetSectionValue<T>(string key)
        {
            return _configuration.GetValue<T>(key);
        }
    }
}
