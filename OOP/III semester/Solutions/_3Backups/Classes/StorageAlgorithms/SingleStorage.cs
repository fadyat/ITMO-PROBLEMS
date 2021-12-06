using System.Collections.Generic;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageMethods;
using Backups.Classes.Storages;

namespace Backups.Classes.StorageAlgorithms
{
    public class SingleStorage : IStorageAlgorithm
    {
        public List<Storage> CreateStorages(string path, IEnumerable<IJobObject> objects, IStorageMethod storageMethod)
        {
            var storage = new Storage("Single", path);
            storageMethod.Archive(objects, storage.FullPath);
            return new List<Storage> { storage };
        }
    }
}