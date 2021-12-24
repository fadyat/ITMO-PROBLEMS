namespace BackupsExtra.Classes.BackupLogs
{
    public interface IMyLogger
    {
        void Info(string message);

        void Warning(string message);

#pragma warning disable CA1716
        void Error(string message);
#pragma warning restore CA1716

        void Debug(string message);
    }
}