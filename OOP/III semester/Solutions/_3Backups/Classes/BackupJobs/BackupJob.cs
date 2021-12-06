using System;
using System.Collections.Generic;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;

namespace Backups.Classes.BackupJobs
{
    public class BackupJob : IBackupJob
    {
        public BackupJob(
            string path,
            HashSet<IJobObject> objects,
            IStorageAlgorithm storageAlgorithm,
            IStorageMethod storageMethod,
            string name = null)
        {
            Name = name ?? Guid.NewGuid().ToString();
            Path = path;
            SetObjects = objects;
            LinkedRestorePoints = new LinkedList<IRestorePoint>();
            StorageAlgorithm = storageAlgorithm;
            StorageMethod = storageMethod;

            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(FullPath);
        }

        public string Path { get; }

        public string Name { get; }

        public string FullPath { get; }

        public IEnumerable<IJobObject> Objects => SetObjects;

        public IEnumerable<IRestorePoint> RestorePoints => LinkedRestorePoints;

        public IStorageAlgorithm StorageAlgorithm { get; }

        public IStorageMethod StorageMethod { get; }

        protected LinkedList<IRestorePoint> LinkedRestorePoints { get; }

        protected HashSet<IJobObject> SetObjects { get; }

        public void AddJobObject(IJobObject jobObject)
        {
            SetObjects.Add(jobObject);
        }

        public void RemoveJobObject(IJobObject jobObject)
        {
            SetObjects.Remove(jobObject);
        }

        public IRestorePoint CreateRestorePoint(IRestorePoint restorePoint)
        {
            LinkedRestorePoints.AddLast(restorePoint);

            // change restore point
            // ...
            return restorePoint;
        }
    }
}