using BackupsExtra.Services;

namespace BackupsExtra.Classes.Serialization
{
    public interface IStorageMethodExtra
    {
        void Save(BackupExtraJobService backupJobService);

        BackupExtraJobService Load();
    }
}