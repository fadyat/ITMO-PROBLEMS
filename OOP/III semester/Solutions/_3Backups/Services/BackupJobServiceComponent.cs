using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Newtonsoft.Json;

namespace Backups.Services
{
    public abstract class BackupJobServiceComponent
    {
        /* for json */
        public string Path { get; protected init; }

        public IStorageMethodComponent StorageMethod { get; protected init; }

        public string Name { get; protected init; }
        /* ---- */

        public ImmutableList<BackupJob> BackupsI => Backups.ToImmutableList();

        public string FullPath { get; protected init; }

        protected HashSet<BackupJob> Backups { get; init; }

        public abstract BackupJob CreateBackup(
            IEnumerable<IJobObject> objects, IStorageAlgorithm storageAlgorithm, string name = null);

        public abstract BackupJob GetBackup(Guid id);
    }
}