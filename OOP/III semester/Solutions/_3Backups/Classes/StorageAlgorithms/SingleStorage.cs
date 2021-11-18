using System.Collections.Generic;
using Backups.Classes.StorageMethods;

namespace Backups.Classes.StorageAlgorithms
{
    public class SingleStorage : IStorageAlgorithm
    {
        public List<Storage> Compression(
            string path,
            IEnumerable<JobObject> objects,
            IStorageMethod storageMethod)
        {
            var storages = new List<Storage>();
            var newStorage = new Storage(
                "Single",
                path,
                objects,
                storageMethod);

            storages.Add(newStorage);
            return storages;
        }
    }
}