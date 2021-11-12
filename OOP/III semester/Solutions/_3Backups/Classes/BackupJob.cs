using System.Collections.Generic;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Exceptions;

namespace Backups.Classes
{
    public class BackupJob
    {
        private readonly HashSet<string> _filePaths;
        private readonly List<RestorePoint> _restorePoints;
        private readonly IStorageAlgorithm _storageAlgorithm;
        private readonly IStorageMethod _storageMethod;

        public BackupJob(
            int id,
            string path,
            HashSet<string> filePaths,
            string name,
            IStorageAlgorithm storageAlgorithm,
            IStorageMethod storageMethod)
        {
            Id = id;
            _filePaths = filePaths;
            _restorePoints = new List<RestorePoint>();
            _storageAlgorithm = storageAlgorithm;
            _storageMethod = storageMethod;
            Path = _storageMethod.ConstructPath(path, name);
            _storageMethod.MakeDirectory(Path);
        }

        public int Id { get; }

        public string Path { get; }

        public IEnumerable<string> FilePaths => _filePaths;

        public void AddFile(string filePath)
        {
            _filePaths.Add(filePath);
        }

        public void RemoveFile(string filePath)
        {
            _filePaths.Remove(filePath);
        }

        public RestorePoint CreateRestorePoint(string name = "restorePoint_")
        {
            if (Equals(_filePaths.Count, 0))
                throw new BackupException("No files for restore point!");

            var restorePoint = new RestorePoint(
                _restorePoints.Count + 1,
                this,
                _storageAlgorithm,
                _storageMethod,
                name);

            _restorePoints.Add(restorePoint);
            return restorePoint;
        }
    }
}