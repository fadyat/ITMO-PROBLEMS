using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
using Backups.Services;
using BackupsExtra.Classes.BackupJobsExtra;
using BackupsExtra.Classes.StorageMethodsExtra;

namespace BackupsExtra.Services
{
    public class BackupExtraJobService : BackupJobServiceDecorator
    {
        public BackupExtraJobService(
            BackupJobServiceComponent backupJobService,
            IStorageExtraMethod storageExtraMethod)
            : base(backupJobService)
        {
            StorageMethod = storageExtraMethod;
            Backups = new HashSet<BackupJobExtra>();
        }

        public new IStorageExtraMethod StorageMethod { get; }

        public new IEnumerable<BackupJobExtra> BackupsI => Backups;

        protected new HashSet<BackupJobExtra> Backups { get; }

        public new BackupJobExtra CreateBackup(
            IEnumerable<IJobObject> objects, IStorageAlgorithm storageAlgorithm, string name = null)
        {
            name ??= Guid.NewGuid().ToString();
            var backupJob =
                new BackupJobExtra(
                    new BackupJob(Guid.NewGuid(), FullPath, objects, storageAlgorithm, StorageMethod, name),
                    StorageMethod);

            var str = backupJob.Path;

            Backups.Add(backupJob);
            StorageMethod.MakeDirectory(backupJob.FullPath);

            return backupJob;
        }

        public new BackupJobExtra GetBackup(Guid id)
        {
            return Backups.Single(b => Equals(b.Id, id));
        }
    }
}