using System;
using System.Collections.Generic;
using Backups.Classes.BackupJobs;
using Backups.Classes.StorageMethods;

namespace Backups.Services
{
    public class BackupJobService : IBackupJobService
    {
        public BackupJobService(string path, IStorageMethod storageMethod, string name = null)
        {
            StorageMethod = storageMethod;
            Backups = new HashSet<IBackupJob>();
            Name = name ?? Guid.NewGuid().ToString();
            Path = path;
            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(FullPath);
        }

        public IEnumerable<IBackupJob> BackupsI => Backups;

        public string Path { get; }

        public string Name { get; }

        public string FullPath { get; }

        public IStorageMethod StorageMethod { get; }

        private HashSet<IBackupJob> Backups { get; }

        public void CreateBackup(IBackupJob backupJob)
        {
            Backups.Add(backupJob);
            StorageMethod.MakeDirectory(backupJob.FullPath);
        }
    }
}