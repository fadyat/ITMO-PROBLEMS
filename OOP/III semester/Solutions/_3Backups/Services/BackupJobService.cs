using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Classes;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;

namespace Backups.Services
{
    public class BackupJobService : IBackupJobService
    {
        private readonly List<BackupJob> _backups;

        public BackupJobService(string path, IStorageMethod storageMethod, string name = "Repository")
        {
            StorageMethod = storageMethod;
            _backups = new List<BackupJob>();
            IssuedBackupId = 100000;
            Name = name;
            Path = path;
            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(Path);
        }

        public IEnumerable<BackupJob> Backups => _backups;

        public string Path { get; }

        public string Name { get; }

        public string FullPath { get; }

        public int IssuedBackupId { get; private set; }

        public IStorageMethod StorageMethod { get; }

        public BackupJob CreateBackup(
            HashSet<JobObject> objects, IStorageAlgorithm storageAlgorithm, string name = "backupJob_")
        {
            name += name.EndsWith("_") ? (_backups.Count + 1).ToString() : string.Empty;

            var backupJob = new BackupJob(
                IssuedBackupId++,
                FullPath,
                objects,
                name,
                storageAlgorithm,
                StorageMethod);

            _backups.Add(backupJob);
            return backupJob;
        }

        public BackupJob GetBackup(int id)
        {
            return _backups.Single(backup => Equals(backup.Id, id));
        }
    }
}