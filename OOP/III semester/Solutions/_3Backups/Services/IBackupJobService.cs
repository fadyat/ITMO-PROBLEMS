using System.Collections.Generic;
using Backups.Classes.BackupJobs;
using Backups.Classes.StorageMethods;
using Newtonsoft.Json;

namespace Backups.Services
{
    public interface IBackupJobService
    {
        IEnumerable<IBackupJob> Backups { get; }

        IStorageMethod StorageMethod { get; }

        string Path { get; }

        string Name { get; }

        [JsonIgnore]
        string FullPath { get; }

        void CreateBackup(IBackupJob backupJob);
    }
}