using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SerialSortTool
{
    public class Program
    {
        public static IHubContext<SerialHub> hubContext;
        public static void Main(string[] args)
        {

            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            if (isService)
            {
                //设置路径
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
                Console.WriteLine(pathToContentRoot);
            }

            var host = CreateWebHostBuilder(args).Build();
            hubContext = (IHubContext <SerialHub>)host.Services.GetService(typeof(IHubContext<SerialHub>));
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(new ConfigTool()["url"])
                .UseStartup<Startup>();
    }
}
