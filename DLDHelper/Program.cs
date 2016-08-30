using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace DLDHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            SetDefaultConfig();
            DownloadMonitor monitor = new DownloadMonitor();
            monitor.Idle();
        }

        static void SetDefaultConfig()
        {
            AppConfig cfg = new AppConfig();

            cfg.Seed();

            Serializer<AppConfig>.Serialize("AppConfig.xml", cfg);
        }
    }
}
