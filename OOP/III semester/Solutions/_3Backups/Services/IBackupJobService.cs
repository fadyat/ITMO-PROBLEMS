using System;
using System.Collections.Generic;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Newtonsoft.Json;

namespace Backups.Services
{
    public interface IBackupJobService
    {
        IEnumerable<IBackupJob> BackupsI { get; }

        IStorageMethod StorageMethod { get; }

        string Path { get; }

        string Name { get; }

        [JsonIgnore]
        string FullPath { get; }

        IBackupJob CreateBackup(IEnumerable<IJobObject> objects, IStorageAlgorithm storageAlgorithm, string name);

        IBackupJob GetBackup(Guid id);
    }
}