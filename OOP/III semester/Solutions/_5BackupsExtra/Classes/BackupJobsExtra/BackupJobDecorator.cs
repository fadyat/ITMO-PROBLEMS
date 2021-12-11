using System;
using System.Collections.Immutable;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Newtonsoft.Json;

namespace BackupsExtra.Classes.BackupJobsExtra
{
    public abstract class BackupJobDecorator : BackupJobComponent
    {
        protected BackupJobDecorator(BackupJobComponent component)
        {
            Component = component;
            Id = Component.Id;
            Path = Component.Path;
            Name = Component.Name;
            FullPath = Component.FullPath;
            StorageAlgorithm = Component.StorageAlgorithm;
            StorageMethod = Component.StorageMethod;
        }

        [JsonIgnore]
        public new ImmutableList<IJobObject> PublicObjects => Component.PublicObjects;

        [JsonIgnore]
        public new ImmutableList<RestorePoint> RestorePoints => Component.RestorePoints;

        [JsonProperty]
        protected BackupJobComponent Component { get; }

        public override void AddJobObject(IJobObject jobObject)
        {
            Component.AddJobObject(jobObject);
        }

        public override void RemoveJobObject(IJobObject jobObject)
        {
            Component.RemoveJobObject(jobObject);
        }

        public override RestorePoint CreateRestorePoint(string name = null, DateTime dateTime = default)
        {
            return Component.CreateRestorePoint(name, dateTime);
        }
    }
}