using Serilog;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;

namespace BackupsExtra.Classes.BackupLogs
{
    public class ConsoleLogger : IMyLogger
    {
        private readonly Logger _logger;

        public ConsoleLogger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Console(theme: ConsoleTheme.None)
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