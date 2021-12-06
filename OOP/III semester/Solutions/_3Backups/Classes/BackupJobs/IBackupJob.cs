using System.Collections.Generic;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;

namespace Backups.Classes.BackupJobs
{
    public interface IBackupJob
    {
        string Path { get; }

        string Name { get; }

        string FullPath { get; }

        IEnumerable<IJobObject> Objects { get; }

        IEnumerable<IRestorePoint> RestorePoints { get; }

        IStorageAlgorithm StorageAlgorithm { get; }

        IStorageMethod StorageMethod { get; }

        void AddJobObject(IJobObject jobObject);

        void RemoveJobObject(IJobObject jobObject);

        IRestorePoint CreateRestorePoint(IRestorePoint restorePoint);
    }
}