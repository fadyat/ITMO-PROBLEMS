using System.Collections.Generic;
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
        private readonly IStorageMethod _storageMethod;

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
            _storageMethod = storageMethod;
            Path = _storageMethod.ConstructPath(path, name);
            _storageMethod.MakeDirectory(Path);
        }

        public int Id { get; }

        public string Path { get; }

        public IEnumerable<JobObject> Objects => _objects;

        public IEnumerable<RestorePoint> RestorePoints => _restorePoints;

        public IStorageAlgorithm StorageAlgorithm { get; }

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

            var restorePoint = new RestorePoint(
                _restorePoints.Count + 1,
                this,
                StorageAlgorithm,
                _storageMethod,
                name);

            _restorePoints.Add(restorePoint);
            return restorePoint;
        }

        public JobObject GetJobObject(string path)
        {
            JobObject jobObject = _objects.SingleOrDefault(jobObject => Equals(jobObject.Path, path));

            if (Equals(jobObject, null))
            {
                throw new BackupException("Job object doesn't exist!");
            }

            return jobObject;
        }
    }
}