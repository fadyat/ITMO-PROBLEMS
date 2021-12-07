using Backups.Services;
using BackupsExtra.Classes.StorageMethodsExtra;

namespace BackupsExtra.Services
{
    public class BackupExtraJobService : BackupJobService
    {
        public BackupExtraJobService(string path, IStorageExtraMethod storageMethod, string name = null)
            : base(path, storageMethod, name)
        {
            StorageMethod = storageMethod;
        }

        public new IStorageExtraMethod StorageMethod { get; }
    }
}