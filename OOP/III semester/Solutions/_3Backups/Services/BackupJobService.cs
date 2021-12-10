using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
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

        public IBackupJob CreateBackup(
            IEnumerable<IJobObject> objects, IStorageAlgorithm storageAlgorithm, string name = null)
        {
            name ??= Guid.NewGuid().ToString();
            var backupJob =
                new BackupJob(Guid.NewGuid(), FullPath, objects, storageAlgorithm, StorageMethod, name);
            Backups.Add(backupJob);
            StorageMethod.MakeDirectory(backupJob.FullPath);

            return backupJob;
        }

        public IBackupJob GetBackup(Guid id)
        {
            return Backups.Single(b => Equals(b.Id, id));
        }
    }
}