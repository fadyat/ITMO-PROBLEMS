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
    public class BackupJob : BackupJobComponent
    {
        public BackupJob(
            Guid id,
            string path,
            IEnumerable<IJobObject> objects,
            IStorageAlgorithm storageAlgorithm,
            IStorageMethodComponent storageMethod,
            string name)
        {
            Id = id;
            Name = name;
            Path = path;
            SetObjects = objects.ToHashSet();
            LinkedRestorePoints = new LinkedList<RestorePoint>();
            StorageAlgorithm = storageAlgorithm;
            StorageMethod = storageMethod;
            FullPath = StorageMethod.ConstructPath(Path, Name);
        }

        public override void AddJobObject(IJobObject jobObject)
        {
            SetObjects.Add(jobObject);
        }

        public override void RemoveJobObject(IJobObject jobObject)
        {
            jobObject = GetJobObject(jobObject.Path);
            SetObjects.Remove(jobObject);
        }

        public override RestorePoint CreateRestorePoint(string name = null, DateTime dateTime = default)
        {
            if (Equals(SetObjects.Count, 0))
                throw new BackupException("No files for restore point!");

            name ??= Guid.NewGuid().ToString();
            string futureRestorePath = System.IO.Path.Combine(FullPath, name);
            StorageMethod.MakeDirectory(futureRestorePath);
            LinkedList<Storage> storages = StorageAlgorithm.CreateStorages(futureRestorePath, SetObjects, StorageMethod);
            var restorePoint = new RestorePoint(FullPath, storages, name, dateTime);
            LinkedRestorePoints.AddLast(restorePoint);

            return restorePoint;
        }

        public IJobObject GetJobObject(string jobObjectPath)
        {
            return SetObjects.Single(obj => Equals(obj.Path, jobObjectPath));
        }
    }
}