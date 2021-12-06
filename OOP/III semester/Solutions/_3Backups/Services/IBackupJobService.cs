using System.Collections.Generic;
using Backups.Classes.BackupJobs;
using Backups.Classes.StorageMethods;

namespace Backups.Services
{
    public interface IBackupJobService
    {
        IEnumerable<IBackupJob> Backups { get; }

        IStorageMethod StorageMethod { get; }

        string Path { get; }

        string Name { get; }

        string FullPath { get; }

        int IssuedBackupId { get; }

        IBackupJob CreateBackup(IBackupJob backupJob);
    }
}