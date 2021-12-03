using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;

namespace Backups.Classes
{
    public class RestorePoint
    {
        private readonly List<Storage> _storages;

        public RestorePoint(
            int id,
            string path,
            IEnumerable<JobObject> backupJobObjects,
            IStorageAlgorithm storageAlgorithm,
            IStorageMethod storageMethod,
            string name)
        {
            StorageMethod = storageMethod;
            Id = id;
            Name = name + (name.EndsWith("_") ? Id.ToString() : string.Empty);
            Path = path;
            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(Path);
            BackupJobObjects = backupJobObjects;
            StorageAlgorithm = storageAlgorithm;
            _storages = StorageAlgorithm.CreateStorages(Path, BackupJobObjects, StorageMethod);
        }

        public int Id { get; }

        public string Name { get; }

        public string Path { get; }

        public IEnumerable<Storage> Storages => _storages;

        public string FullPath { get; }

        public IStorageMethod StorageMethod { get; }

        public IEnumerable<JobObject> BackupJobObjects { get; }

        public IStorageAlgorithm StorageAlgorithm { get; }
    }
}