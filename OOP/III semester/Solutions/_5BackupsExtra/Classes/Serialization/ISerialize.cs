using BackupsExtra.Services;

namespace BackupsExtra.Classes.Serialization
{
    public interface ISerialize
    {
        void Save(BackupExtraJobService backupJobService);

        BackupExtraJobService Load();
    }
}