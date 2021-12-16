using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

namespace BackupsExtra.Classes.BackupLogs
{
    public class FileLogger : IMyLogger
    {
        private readonly Logger _logger;

        public FileLogger(string path, string name = "backupLogger.txt")
        {
            Path = path;
            Name = name;
            _logger = new LoggerConfiguration()
                .WriteTo.File(System.IO.Path.Combine(Path, Name))
                .CreateLogger();
        }

        [JsonProperty]
        private string Path { get; }

        [JsonProperty]
        private string Name { get; }

        public void Info(string message)
        {
            _logger.Information(message);
        }

        public void Warning(string message)
        {
            _logger.Warning(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }
    }
}