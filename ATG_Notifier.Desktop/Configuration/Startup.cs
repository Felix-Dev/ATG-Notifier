using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Configuration
{
    internal static class Startup
    {
        private static readonly ServiceCollection serviceCollection = new ServiceCollection();

        public static void Configure()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ServiceLocator.Configure(serviceCollection);

            // Needed because EF Core Sqlite Provider cannot create folders
            if (!Directory.Exists(AppConfiguration.BaseDirectory))
            {
                Directory.CreateDirectory(AppConfiguration.BaseDirectory);
            }
        }
    }
}
