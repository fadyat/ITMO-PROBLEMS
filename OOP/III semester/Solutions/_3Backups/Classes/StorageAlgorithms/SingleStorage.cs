using System.Collections.Generic;
using Backups.Classes.StorageMethods;

namespace Backups.Classes.StorageAlgorithms
{
    public class SingleStorage : IStorageAlgorithm
    {
        public List<Storage> CreateStorages(string path, IEnumerable<JobObject> objects, IStorageMethod storageMethod)
        {
            var storage = new Storage("Single", path, storageMethod);
            storageMethod.Archive(objects, storage.FullPath);
            return new List<Storage> { storage };
        }
    }
}