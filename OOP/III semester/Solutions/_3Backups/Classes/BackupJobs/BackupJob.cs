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
            IStorageMethod storageMethod,
            string name)
        {
            Id = id;
            Path = path;
            Objects = objects.ToHashSet();
            StorageAlgorithm = storageAlgorithm;
            StorageMethod = storageMethod;
            Name = name;
            LinkedRestorePoints = new LinkedList<RestorePoint>();
            FullPath = StorageMethod.ConstructPath(Path, Name);
        }

        public override void AddJobObject(IJobObject jobObject)
        {
            Objects.Add(jobObject);
        }

        public override void RemoveJobObject(IJobObject jobObject)
        {
            jobObject = GetJobObject(jobObject.Path);
            Objects.Remove(jobObject);
        }

        public override RestorePoint CreateRestorePoint(string name = null, DateTime dateTime = default)
        {
            if (Equals(PublicObjects.Count, 0))
                throw new BackupException("No files for restore point!");

            name ??= Guid.NewGuid().ToString();
            string futureRestorePath = System.IO.Path.Combine(FullPath, name);
            StorageMethod.MakeDirectory(futureRestorePath);
            LinkedList<Storage> storages = StorageAlgorithm.CreateStorages(futureRestorePath, PublicObjects, StorageMethod);
            var restorePoint = new RestorePoint(FullPath, storages, name, dateTime);
            LinkedRestorePoints.AddLast(restorePoint);

            return restorePoint;
        }

        private IJobObject GetJobObject(string jobObjectPath)
        {
            return Objects.Single(obj => Equals(obj.Path, jobObjectPath));
        }
    }
}