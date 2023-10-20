using BackupsExtra.Services;

namespace BackupsExtra.Classes.Serialization
{
    public interface ISerialize
    {
        void Save(BackupJobServiceExtra backupJobService);

        BackupJobServiceExtra Load();
    }
}