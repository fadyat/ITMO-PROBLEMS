using Backups.Services;

namespace BackupsExtra.Classes.Serialization
{
    public interface IStorageMethodExtra
    {
        void Save(IBackupJobService backupJobService);

        IBackupJobService Load();
    }
}