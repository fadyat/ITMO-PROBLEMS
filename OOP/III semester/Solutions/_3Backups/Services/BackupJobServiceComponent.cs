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
        /* for json constructor */
        public string Path { get; protected init; }

        public IStorageMethod StorageMethod { get; protected init; }

        public string Name { get; protected init; }
        /* ---- */

        /* for json adding backup */
        [JsonIgnore]
        public ImmutableList<BackupJob> BackupsI => Backups.ToImmutableList();

        [JsonProperty] // to protected (works with public) IGNORE?
        public HashSet<BackupJob> Backups { get; init; }
        /* ---- */

#pragma warning disable SA1202
        public string FullPath { get; protected init; }
#pragma warning restore SA1202

        public abstract BackupJob CreateBackup(
            IEnumerable<IJobObject> objects, IStorageAlgorithm storageAlgorithm, string name = null);

        public abstract BackupJob GetBackup(Guid id);
    }
}