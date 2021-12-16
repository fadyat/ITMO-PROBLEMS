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
        public Guid Id { get; protected init; }

        public string Path { get; protected init; }

        public IStorageAlgorithm StorageAlgorithm { get; protected init; }

        public IStorageMethod StorageMethod { get; protected init; }

        public string Name { get; protected init; }

        [JsonIgnore]
        public string FullPath { get; protected init; }

        [JsonIgnore]
        public ImmutableList<IJobObject> PublicObjects => Objects.ToImmutableList();

        [JsonIgnore]
        public ImmutableList<RestorePoint> RestorePoints => LinkedRestorePoints.ToImmutableList();

        [JsonProperty]
        protected LinkedList<RestorePoint> LinkedRestorePoints { get; set; }

        [JsonProperty]
        protected HashSet<IJobObject> Objects { get; set; }

        public abstract void AddJobObject(IJobObject jobObject);

        public abstract void RemoveJobObject(IJobObject jobObject);

        public abstract RestorePoint CreateRestorePoint(string name = null, DateTime dateTime = default);
    }
}