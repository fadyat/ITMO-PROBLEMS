using System;
using System.Collections.Generic;
using Backups.Classes.BackupJobs;
using Backups.Classes.StorageMethods;

namespace Backups.Services
{
    public class BackupJobService : IBackupJobService
    {
        private readonly HashSet<IBackupJob> _backups;

        public BackupJobService(string path, IStorageMethod storageMethod, string name = null)
        {
            StorageMethod = storageMethod;
            _backups = new HashSet<IBackupJob>();
            Name = name ?? Guid.NewGuid().ToString();
            Path = path;
            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(FullPath);
        }

        public IEnumerable<IBackupJob> Backups => _backups;

        public string Path { get; }

        public string Name { get; }

        public string FullPath { get; }

        public IStorageMethod StorageMethod { get; }

        public void CreateBackup(IBackupJob backupJob)
        {
            _backups.Add(backupJob);
            StorageMethod.MakeDirectory(backupJob.FullPath);
        }
    }
}