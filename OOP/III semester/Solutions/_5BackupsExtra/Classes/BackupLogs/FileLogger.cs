using System.IO;
using Serilog;
using Serilog.Core;

namespace BackupsExtra.Classes.BackupLogs
{
    public class FileLogger : IMyLogger
    {
        private readonly Logger _logger;

        public FileLogger(string path, string toFile = "backupLogger.txt")
        {
            _logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(path, toFile))
                .CreateLogger();
        }

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