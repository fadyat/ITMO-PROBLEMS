using System;
using System.Collections.Generic;
using Backups.Classes.BackupJobs;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
using Backups.Services;
using Newtonsoft.Json;

namespace BackupsExtra.Services
{
    public abstract class BackupJobServiceDecorator : BackupJobServiceComponent
    {
        protected BackupJobServiceDecorator(BackupJobServiceComponent component)
        {
            Component = component;
            Name = Component.Name;
            Path = Component.Path;
            FullPath = Component.FullPath;
            StorageMethod = Component.StorageMethod;
        }

        [JsonProperty]
        protected BackupJobServiceComponent Component { get; }

        public override BackupJob CreateBackup(
            IEnumerable<IJobObject> objects, IStorageAlgorithm storageAlgorithm, string name = null)
        {
            return Component.CreateBackup(objects, storageAlgorithm, name);
        }

        public override BackupJob GetBackup(Guid id)
        {
            return Component.GetBackup(id);
        }
    }
}