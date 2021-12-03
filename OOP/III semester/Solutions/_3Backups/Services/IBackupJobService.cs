using System.Collections.Generic;
using Backups.Classes;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;

namespace Backups.Services
{
    public interface IBackupJobService
    {
        IEnumerable<BackupJob> Backups { get; }

        IStorageMethod StorageMethod { get; }

        string Location { get; }

        int IssuedBackupId { get; }

        BackupJob CreateBackup(
            HashSet<JobObject> objects,
            IStorageAlgorithm storageAlgorithm,
            string name = "backupJob_");

        BackupJob GetBackup(int id);
    }
}