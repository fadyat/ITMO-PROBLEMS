using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Exceptions;

namespace Backups.Classes
{
    public class BackupJob
    {
        private readonly HashSet<JobObject> _objects;
        private readonly List<RestorePoint> _restorePoints;

        public BackupJob(
            int id,
            string path,
            HashSet<JobObject> objects,
            string name,
            IStorageAlgorithm storageAlgorithm,
            IStorageMethod storageMethod)
        {
            Id = id;
            _objects = objects;
            _restorePoints = new List<RestorePoint>();
            StorageAlgorithm = storageAlgorithm;
            StorageMethod = storageMethod;
            Path = path;
            Name = name;
            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(Path);
        }

        public int Id { get; }

        public string Path { get; }

        public string Name { get; }

        public string FullPath { get; }

        public IEnumerable<JobObject> Objects => _objects;

        public IEnumerable<RestorePoint> RestorePoints => _restorePoints;

        public IStorageAlgorithm StorageAlgorithm { get; }

        public IStorageMethod StorageMethod { get; }

        public void AddJobObject(JobObject jobObject)
        {
            _objects.Add(jobObject);
        }

        public void RemoveJobObject(JobObject jobObject)
        {
            _objects.Remove(jobObject);
        }

        public RestorePoint CreateRestorePoint(string name = "restorePoint_")
        {
            if (Equals(_objects.Count, 0))
                throw new BackupException("No files for restore point!");

            int restoreNumber = _restorePoints.Count + 1;
            var restorePoint = new RestorePoint(restoreNumber, FullPath, _objects, StorageAlgorithm, StorageMethod, name);

            _restorePoints.Add(restorePoint);
            return restorePoint;
        }

        public JobObject GetJobObject(string path)
        {
            return _objects.Single(jobObject => Equals(jobObject.Path, path));
        }
    }
}