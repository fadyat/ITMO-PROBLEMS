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

        public string Name { get; protected init; }

        [JsonIgnore]
        public string FullPath { get; protected init; }

        public ImmutableList<IJobObject> Objects => SetObjects.ToImmutableList();

        public ImmutableList<RestorePoint> RestorePoints => LinkedRestorePoints.ToImmutableList();

        public IStorageAlgorithm StorageAlgorithm { get; protected init; }

        public IStorageMethodComponent StorageMethod { get; protected init; }

        protected LinkedList<RestorePoint> LinkedRestorePoints { get; set; }

        protected HashSet<IJobObject> SetObjects { get; set; }

        public abstract void AddJobObject(IJobObject jobObject);

        public abstract void RemoveJobObject(IJobObject jobObject);

        public abstract RestorePoint CreateRestorePoint(string name = null, DateTime dateTime = default);
    }
}