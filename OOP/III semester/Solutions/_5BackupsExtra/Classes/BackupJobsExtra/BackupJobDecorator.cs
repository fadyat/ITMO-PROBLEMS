using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;

namespace BackupsExtra.Classes.BackupJobsExtra
{
    public abstract class BackupJobDecorator : BackupJobComponent
    {
        protected BackupJobDecorator(BackupJobComponent component)
        {
            Component = component;
            Id = component.Id;
            Path = component.Path;
            Name = component.Name;
            FullPath = component.FullPath;
            StorageAlgorithm = component.StorageAlgorithm;
            StorageMethod = component.StorageMethod;
            LinkedRestorePoints = component.RestorePoints as LinkedList<RestorePoint>;

            // SetObjects;
        }

        public new IEnumerable<IJobObject> Objects => Component.Objects;

        public new IEnumerable<RestorePoint> RestorePoints => Component.RestorePoints;

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