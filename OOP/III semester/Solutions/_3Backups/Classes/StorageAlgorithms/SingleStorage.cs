using System.Collections.Generic;
using System.Linq;
using Backups.Classes.JobObjects;
using Backups.Classes.StorageMethods;
using Backups.Classes.Storages;

namespace Backups.Classes.StorageAlgorithms
{
    public class SingleStorage : IStorageAlgorithm
    {
        public List<Storage> CreateStorages(string path, IEnumerable<IJobObject> objects, IStorageMethod storageMethod)
        {
            IEnumerable<IJobObject> jobObjects = objects.ToList();
            var storage = new Storage("Single.zip", path, jobObjects);
            storageMethod.Archive(storage);
            return new List<Storage> { storage };
        }
    }
}