using Microsoft.Extensions.Configuration;

namespace MusicApp
{
    static class Program
    {
        public static IConfiguration Configuration { get; private set; }
        public static string AudioDbApiKey => Configuration?["ApiKey"];

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Build configuration to load User Secrets
            var builder = new ConfigurationBuilder().AddUserSecrets("ApiKey");
            Configuration = builder.Build();

            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetColorMode(SystemColorMode.Dark);

            Application.Run(new Index());
        }
    }
}