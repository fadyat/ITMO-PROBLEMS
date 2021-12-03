using System.Collections.Generic;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Exceptions;
using Newtonsoft.Json;

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
            Name = name;
            Path = path;
            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(FullPath);
        }

        public int Id { get; }

        public string Path { get; }

        public string Name { get; }

        [JsonIgnore]
        public string FullPath { get; }

        public IEnumerable<JobObject> Objects => _objects;

        public IEnumerable<RestorePoint> RestorePoints => _restorePoints;

        [JsonProperty]
        private IStorageAlgorithm StorageAlgorithm { get; }

        [JsonProperty]
        private IStorageMethod StorageMethod { get; }

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
            var restorePoint =
                new RestorePoint(restoreNumber, FullPath, _objects, StorageAlgorithm, StorageMethod, name);

            _restorePoints.Add(restorePoint);
            return restorePoint;
        }
    }
}