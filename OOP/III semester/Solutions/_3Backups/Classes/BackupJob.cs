using System;
using System.Collections.Generic;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Exceptions;

namespace Backups.Classes
{
    public class BackupJob
    {
        public BackupJob(
            int id,
            string path,
            HashSet<JobObject> objects,
            string name,
            IStorageAlgorithm storageAlgorithm,
            IStorageMethod storageMethod)
        {
            Id = id;
            SetObjects = objects;
            LinkedRestorePoints = new LinkedList<RestorePoint>();
            StorageAlgorithm = storageAlgorithm;
            StorageMethod = storageMethod;
            Name = name;
            Path = path;
            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(FullPath);
        }

        public int Id { get; }

        public string Path { get; }

        public string Name { get; }

        public string FullPath { get; }

        public IEnumerable<JobObject> Objects => SetObjects;

        public IEnumerable<RestorePoint> RestorePoints => LinkedRestorePoints;

        public IStorageAlgorithm StorageAlgorithm { get; }

        public IStorageMethod StorageMethod { get; }

        protected LinkedList<RestorePoint> LinkedRestorePoints { get; set; }

        protected HashSet<JobObject> SetObjects { get; }

        public void AddJobObject(JobObject jobObject)
        {
            SetObjects.Add(jobObject);
        }

        public void RemoveJobObject(JobObject jobObject)
        {
            SetObjects.Remove(jobObject);
        }

        public RestorePoint CreateRestorePoint(DateTime creationDate = default, string name = "restorePoint_")
        {
            if (creationDate == default)
                creationDate = DateTime.Now;

            if (Equals(SetObjects.Count, 0))
                throw new BackupException("No files for restore point!");

            int restoreNumber = LinkedRestorePoints.Count + 1;
            var restorePoint =
                new RestorePoint(restoreNumber, FullPath, SetObjects, StorageAlgorithm, StorageMethod, name, creationDate);

            LinkedRestorePoints.AddLast(restorePoint);
            return restorePoint;
        }
    }
}