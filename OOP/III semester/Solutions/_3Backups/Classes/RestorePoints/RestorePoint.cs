using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Classes.Storages;
using Backups.Exceptions;
using Newtonsoft.Json;

namespace Backups.Classes.RestorePoints
{
    public class RestorePoint : IRestorePoint
    {
        public RestorePoint(
            string path,
            IEnumerable<IJobObject> backupJobObjects,
            IStorageAlgorithm storageAlgorithm,
            IStorageMethod storageMethod,
            DateTime dateTime = default,
            string name = null)
        {
            var jobObjects = backupJobObjects.ToList();

            Path = path;
            Name = name ?? Guid.NewGuid().ToString();
            StorageMethod = storageMethod;
            BackupJobObjects = jobObjects;
            StorageAlgorithm = storageAlgorithm;
            CreationDate = dateTime;

            if (Equals(jobObjects.Count, 0))
                throw new BackupException("No files for restore point!");

            FullPath = StorageMethod.ConstructPath(Path, Name);
            StorageMethod.MakeDirectory(FullPath);
            Storages = StorageAlgorithm.CreateStorages(FullPath, jobObjects, StorageMethod);
        }

        public string Name { get; }

        public string Path { get; }

        public DateTime CreationDate { get; }

        public string FullPath { get; }

        public IEnumerable<Storage> StoragesI => Storages;

        protected List<Storage> Storages { get; }

        [JsonProperty]
        protected IEnumerable<IJobObject> BackupJobObjects { get; }

        [JsonProperty]
        protected IStorageAlgorithm StorageAlgorithm { get; }

        [JsonProperty]
        protected IStorageMethod StorageMethod { get; }
    }
}