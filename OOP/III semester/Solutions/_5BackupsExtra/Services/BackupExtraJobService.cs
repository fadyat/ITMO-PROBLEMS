using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
using Backups.Services;
using BackupsExtra.Classes.BackupJobsExtra;
using BackupsExtra.Classes.StorageMethodsExtra;
using Newtonsoft.Json;

namespace BackupsExtra.Services
{
    public class BackupExtraJobService : BackupJobServiceDecorator
    {
        public BackupExtraJobService(BackupJobServiceComponent component, IStorageExtraMethod storageMethod)
            : base(component)
        {
            StorageMethod = storageMethod;
            Backups = new HashSet<BackupJobExtra>();
        }

        public new IStorageExtraMethod StorageMethod { get; }

        [JsonIgnore]
        public new ImmutableList<BackupJobExtra> BackupsI => Backups.ToImmutableList();

        [JsonProperty] // to protected (works with public)
        public new HashSet<BackupJobExtra> Backups { get; init; }

        public new BackupJobExtra CreateBackup(
            IEnumerable<IJobObject> objects, IStorageAlgorithm storageAlgorithm, string name = null)
        {
            name ??= Guid.NewGuid().ToString();
            var backupJob =
                new BackupJobExtra(
                    new BackupJob(Guid.NewGuid(), FullPath, objects, storageAlgorithm, StorageMethod, name),
                    StorageMethod);

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