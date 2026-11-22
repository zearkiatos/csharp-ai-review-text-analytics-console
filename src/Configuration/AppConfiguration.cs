using dotenv.net;

namespace AIReviewTextAnalyticsConsole.Configuration
{
    public static class AppConfiguration
    {
        internal static Func<DotEnvOptions, IDictionary<string, string>> ReadEnv = DotEnv.Read;
        internal static Action<DotEnvOptions> LoadEnv = DotEnv.Load;

        private static IDictionary<string, string>? envVars;

        private static IDictionary<string, string> EnvVars => envVars ??= Initialize();

        private static IDictionary<string, string> Initialize()
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            var envFileName = ".env";
            if (environment == "test")
                envFileName = ".env.test";
            else if (environment == "local") {
                envFileName = ".env.local";
            }
            var envFile = ResolveEnvFilePath(envFileName);
            Console.WriteLine($"Loading environment file: {envFile}");
            var options = new DotEnvOptions(envFilePaths: new[] { envFile });
            LoadEnv(options);
            return ReadEnv(options);
        }

        private static string ResolveEnvFilePath(string fileName)
        {
            foreach (var startDir in new[] { AppContext.BaseDirectory, Directory.GetCurrentDirectory() })
            {
                var dir = new DirectoryInfo(startDir);
                for (var i = 0; dir is not null && i < 6; i++, dir = dir.Parent)
                {
                    var candidate = Path.Combine(dir.FullName, fileName);
                    if (File.Exists(candidate))
                        return candidate;
                }
            }
            return fileName;
        }
        internal static void ResetForTests() => envVars = null;

        public static string environment => 
            EnvVars["ENVIRONMENT"] 
            ?? throw new InvalidOperationException("ENVIRONMENT environment variable is not set");

        public static string azureBaseUrl => 
            EnvVars["AZURE_BASE_URL"] 
            ?? throw new InvalidOperationException("AZURE_BASE_URL environment variable is not set");

        public static string azureKeyCredential =>
            EnvVars["AZURE_KEY_CREDENTIAL"] 
            ?? throw new InvalidOperationException("AZURE_KEY_CREDENTIAL environment variable is not set");

        public static void ValidateConfiguration()
        {
            try
            {
                _ = environment;
                _ = azureBaseUrl;
                _ = azureKeyCredential; 
                Console.WriteLine("✓ Configuration validation passed");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"✗ Configuration validation failed: {ex.Message}");
                throw;
            }
        }
    }
    
}