using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;

namespace Backups.Services
{
    public class BackupJobService : BackupJobServiceComponent
    {
        public BackupJobService(string path, IStorageMethod storageMethod, string name = null)
        {
            Path = path;
            StorageMethod = storageMethod;
            Name = name ?? Guid.NewGuid().ToString();
            Backups = new HashSet<BackupJob>();
            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(FullPath);
        }

        public override BackupJob CreateBackup(
            IEnumerable<IJobObject> objects, IStorageAlgorithm storageAlgorithm, string name = null)
        {
            name ??= Guid.NewGuid().ToString();
            var backupJob =
                new BackupJob(Guid.NewGuid(), FullPath, objects, storageAlgorithm, StorageMethod, name);

            Backups.Add(backupJob);
            StorageMethod.MakeDirectory(backupJob.FullPath);

            return backupJob;
        }

        public override BackupJob GetBackup(Guid id)
        {
            return Backups.Single(b => Equals(b.Id, id));
        }
    }
}