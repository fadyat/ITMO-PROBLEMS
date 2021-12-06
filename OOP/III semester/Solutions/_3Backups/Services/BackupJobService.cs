using System.Collections.Generic;
using System.Linq;
using Backups.Classes;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;

namespace Backups.Services
{
    public class BackupJobService : IBackupJobService
    {
        private readonly List<IBackupJob> _backups;

        public BackupJobService(string path, IStorageMethod storageMethod, string name = "Repository")
        {
            StorageMethod = storageMethod;
            _backups = new List<IBackupJob>();
            IssuedBackupId = 100000;
            Name = name;
            Path = path;
            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(Path);
        }

        public IEnumerable<IBackupJob> Backups => _backups;

        public string Path { get; }

        public string Name { get; }

        public string FullPath { get; }

        public int IssuedBackupId { get; private set; }

        public IStorageMethod StorageMethod { get; }

        public IBackupJob CreateBackup(IBackupJob backupJob)
        {
            _backups.Add(backupJob);

            // change
            // ...
            return backupJob;
        }
    }
}