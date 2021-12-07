using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Classes.Storages;
using Backups.Exceptions;

namespace Backups.Classes.BackupJobs
{
    public class BackupJob : IBackupJob
    {
        public BackupJob(
            string path,
            IEnumerable<IJobObject> objects,
            IStorageAlgorithm storageAlgorithm,
            IStorageMethod storageMethod,
            string name = null)
        {
            Name = name ?? Guid.NewGuid().ToString();
            Path = path;
            SetObjects = objects.ToHashSet();
            LinkedRestorePoints = new LinkedList<IRestorePoint>();
            StorageAlgorithm = storageAlgorithm;
            StorageMethod = storageMethod;
            FullPath = StorageMethod.ConstructPath(Path, Name);
        }

        public string Path { get; }

        public string Name { get; }

        public string FullPath { get; }

        public IEnumerable<IJobObject> Objects => SetObjects;

        public IEnumerable<IRestorePoint> RestorePoints => LinkedRestorePoints;

        public IStorageAlgorithm StorageAlgorithm { get; }

        public IStorageMethod StorageMethod { get; }

        protected LinkedList<IRestorePoint> LinkedRestorePoints { get; set; }

        protected HashSet<IJobObject> SetObjects { get; set; }

        public void AddJobObject(IJobObject jobObject)
        {
            SetObjects.Add(jobObject);
        }

        public void RemoveJobObject(IJobObject jobObject)
        {
            jobObject = GetJobObject(jobObject.Path);
            SetObjects.Remove(jobObject);
        }

        public void CreateRestorePoint(IRestorePoint restorePoint)
        {
            if (!StorageMethod.ExistsDirectory(FullPath))
                throw new BackupException("This backup wasn't registered!");

            if (Equals(SetObjects.Count, 0))
                throw new BackupException("No files for restore point!");

            StorageMethod.MakeDirectory(restorePoint.FullPath);
            List<Storage> storages = StorageAlgorithm.CreateStorages(restorePoint.FullPath, SetObjects, StorageMethod);
            foreach (Storage storage in storages)
            {
                restorePoint.AddStorage(storage);
            }

            LinkedRestorePoints.AddLast(restorePoint);
        }

        public IJobObject GetJobObject(string jobObjectPath)
        {
            return SetObjects.Single(obj => Equals(obj.Path, jobObjectPath));
        }
    }
}