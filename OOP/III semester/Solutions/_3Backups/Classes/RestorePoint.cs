using System;
using System.Collections.Generic;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Newtonsoft.Json;

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
            string name,
            DateTime dateTime)
        {
            StorageMethod = storageMethod;
            Id = id;
            Name = name + (name.EndsWith("_") ? Id.ToString() : string.Empty);
            Path = path;
            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(FullPath);
            BackupJobObjects = backupJobObjects;
            StorageAlgorithm = storageAlgorithm;
            _storages = StorageAlgorithm.CreateStorages(FullPath, BackupJobObjects, StorageMethod);

            CreationDate = dateTime;
        }

        public string Name { get; }

        public string Path { get; }

        public DateTime CreationDate { get; }

        [JsonIgnore]
        public string FullPath { get; }

        public IEnumerable<Storage> Storages => _storages;

        [JsonProperty]
        private int Id { get; }

        [JsonProperty]
        private IEnumerable<JobObject> BackupJobObjects { get; }

        [JsonProperty]
        private IStorageAlgorithm StorageAlgorithm { get; }

        [JsonProperty]
        private IStorageMethod StorageMethod { get; }
    }
}