using Backups.Services;

namespace BackupsExtra.Classes
{
    public interface IStorageMethodExtra
    {
        void Save(BackupJobService backupJobService);

        BackupJobService Loading();
    }
}