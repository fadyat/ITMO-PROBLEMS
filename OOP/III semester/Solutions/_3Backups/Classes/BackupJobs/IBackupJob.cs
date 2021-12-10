using System;
using System.Collections.Generic;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Newtonsoft.Json;

namespace Backups.Classes.BackupJobs
{
    public interface IBackupJob
    {
        Guid Id { get; }

        string Path { get; }

        string Name { get; }

        [JsonIgnore]
        string FullPath { get; }

        IEnumerable<IJobObject> Objects { get; }

        IEnumerable<RestorePoint> RestorePoints { get; }

        IStorageAlgorithm StorageAlgorithm { get; }

        IStorageMethod StorageMethod { get; }

        void AddJobObject(IJobObject jobObject);

        void RemoveJobObject(IJobObject jobObject);

        RestorePoint CreateRestorePoint(string name = null, DateTime dateTime = default);
    }
}