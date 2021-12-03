using System.Collections.Generic;
using Backups.Classes;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Newtonsoft.Json;

namespace Backups.Services
{
    public interface IBackupJobService
    {
        IEnumerable<BackupJob> Backups { get; }

        IStorageMethod StorageMethod { get; }

        string Path { get; }

        string Name { get; }

        [JsonIgnore]
        string FullPath { get; }

        int IssuedBackupId { get; }

        BackupJob CreateBackup(
            HashSet<JobObject> objects,
            IStorageAlgorithm storageAlgorithm,
            string name = "backupJob_");

        BackupJob GetBackup(int id);
    }
}