using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Newtonsoft.Json;

namespace Backups.Classes.BackupJobs
{
    public abstract class BackupJobComponent
    {
        /* for json */
        public Guid Id { get; protected init; }

        public string Path { get; protected init; }

        [JsonProperty]
        protected HashSet<IJobObject> Objects { get; set; }

#pragma warning disable SA1202
        public IStorageAlgorithm StorageAlgorithm { get; protected init; }
#pragma warning restore SA1202

        public IStorageMethod StorageMethod { get; protected init; }

        public string Name { get; protected init; }
        /* ---- */

        public string FullPath { get; protected init; }

        [JsonIgnore]
        public ImmutableList<IJobObject> PublicObjects => Objects.ToImmutableList();

        [JsonIgnore]
        public ImmutableList<RestorePoint> RestorePoints => LinkedRestorePoints.ToImmutableList();

        [JsonProperty]
        protected LinkedList<RestorePoint> LinkedRestorePoints { get; set; }

        public abstract void AddJobObject(IJobObject jobObject);

        public abstract void RemoveJobObject(IJobObject jobObject);

        public abstract RestorePoint CreateRestorePoint(string name = null, DateTime dateTime = default);
    }
}